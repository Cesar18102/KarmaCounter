using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Cryptography;

using Microsoft.CodeAnalysis.CSharp.Scripting;

using Newtonsoft.Json;

using Autofac;

using KarmaCounterServer.Dto;
using KarmaCounterServer.Model;
using KarmaCounterServer.Exceptions;
using KarmaCounterServer.DataAccess;

namespace KarmaCounterServer.Services
{
    public class RuleService
    {
        public async Task<Rule> AddRule(RuleForm ruleForm)
        {
            if (!(await Global.DI.Resolve<SessionService>().CheckSession(ruleForm.CreatorSession)).Result) //if user is unauthorized
                throw new InvalidSessionException();

            Group group = await Global.DI.Resolve<GroupService>().GetById(ruleForm.GroupId); //may throw not found exception
            Rule rule = new Rule(ruleForm.Title, ruleForm.Text, ruleForm.FeeFormula, group);

            if (group.Owner.Id != ruleForm.CreatorSession.UserId)
                throw new ForbiddenException("You're not creator of the group");

            return await Global.DI.Resolve<RuleDataAccess>().Create(rule);
        }

        public async Task<Rule> GetById(long ruleId)
        {
            Rule rule = await Global.DI.Resolve<RuleDataAccess>().GetById(ruleId);

            if (rule == null)
                throw new NotFoundException("Rule");

            return rule;
        }

        private double Retrieve(dynamic data, List<string> index) => 
            index.Count == 0 ? data : Retrieve(data[index[0]], index.Skip(1).ToList());

        private async Task<RuleAction> AccountRuleAction(RuleActionForm actionForm, bool violated)
        {
            Group group = await Global.DI.Resolve<GroupService>().GetByPublicKey(actionForm.PublicKey); //may throw not found exception
            CheckKeys(actionForm, group.Rights); //may throw forbidden exception

            Membership membership = group.Members.SingleOrDefault(M => M.Member.Id == actionForm.UserId);
            Rule rule = group.Rules.SingleOrDefault(R => R.Id == actionForm.RuleId);
            User user = membership.Member;

            if (user == null)
                throw new NotFoundException("Member");

            if(rule == null)
                throw new NotFoundException("Rule");

            dynamic customData = JsonConvert.DeserializeObject(membership.CustomData);
            string expression = rule.FeeFormula;

            for (int i = 0; i < actionForm.Values?.Count; i++)
            {
                double val = 0;

                if(!double.TryParse(actionForm.Values[i].ToString(), out val))
                    val = Retrieve(customData, actionForm.Values[i].ToString().Split('.').ToList());

                expression = expression.Replace("{" + i + "}", val.ToString().Replace(",", "."));
            }

            if (expression.Contains("{"))
                throw new BadRequestException("Not enough parameters");

            double fee = (violated ? -1 : 1) * (await CSharpScript.EvaluateAsync<double>(expression));
            RuleAction action = new RuleAction(user, rule, DateTime.Now, violated, fee);

            if (group.IsLocal) //affecting membership karma
                await Global.DI.Resolve<MembershipDataAccess>().Update(new Membership(new Membership(membership, group), membership.LocalKarma + fee));
            else //affecting user global karma
                await Global.DI.Resolve<UserDataAccess>().Update(new User(user, user.GlobalKarma + fee));

            return await Global.DI.Resolve<RuleActionDataAccess>().Create(action);
        }

        public async Task<RuleAction> AccountRuleViolation(RuleActionForm actionForm) => 
            await AccountRuleAction(actionForm, true);

        public async Task<RuleAction> AccountRuleAdherence(RuleActionForm actionForm) =>
            await AccountRuleAction(actionForm, false);

        private static readonly MD5 mD5 = MD5.Create();
        private void CheckKeys(RuleActionForm actionForm, Ownership rights)
        {
            string rawHash = $"{rights.PrivateKey}_{actionForm.RuleId}_{actionForm.UserId}_{actionForm.PublicKey}_{rights.PrivateKey}";
            string hash = BitConverter.ToString(mD5.ComputeHash(Encoding.UTF8.GetBytes(rawHash))).Replace("-", "").ToUpper();

            if (hash != actionForm.Hash)
                throw new ForbiddenException("Invalid hash");
        }
    }
}