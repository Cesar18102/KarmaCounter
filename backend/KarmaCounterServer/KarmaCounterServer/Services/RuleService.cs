using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;

using Microsoft.CodeAnalysis.CSharp.Scripting;

using Autofac;

using KarmaCounterServer.Dto;
using KarmaCounterServer.Model;
using KarmaCounterServer.Exceptions;
using KarmaCounterServer.DataAccess;

namespace KarmaCounterServer.Services
{
    public class RuleService
    {
        private static readonly MD5 mD5 = MD5.Create();

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

        private async Task<RuleAction> AccountRuleAction(RuleActionForm actionForm, bool violated)
        {
            Group group = await Global.DI.Resolve<GroupService>().GetByPublicKey(actionForm.PublicKey);
            CheckKeys(actionForm, group.Rights); //may throw forbidden exception

            User user = group.Members.SingleOrDefault(M => M.Member.Id == actionForm.UserId).Member;
            Rule rule = group.Rules.SingleOrDefault(R => R.Id == actionForm.RuleId);

            if (user == null)
                throw new NotFoundException("Member");

            if(rule == null)
                throw new NotFoundException("Rule");

            string expression = rule.FeeFormula;
            for (int i = 0; i < actionForm.Values?.Count; i++)
                expression = expression.Replace("{" + i + "}", actionForm.Values[i].ToString().Replace(",", "."));

            if (expression.Contains("{"))
                throw new BadRequestException("Not enough parameters");

            RuleAction action = new RuleAction(user, rule, DateTime.Now, violated, await CSharpScript.EvaluateAsync<double>(expression));

            //if(group.IsLocal) update membership
            //else update user

            return await Global.DI.Resolve<RuleActionDataAccess>().Create(action);
        }

        public async Task<RuleAction> AccountRuleViolation(RuleActionForm actionForm)
        {
            return await AccountRuleAction(actionForm, true);
        }

        public async Task<RuleAction> AccountRuleAdherence(RuleActionForm actionForm)
        {
            return await AccountRuleAction(actionForm, false);
        }

        private void CheckKeys(RuleActionForm actionForm, Ownership rights)
        {
            string rawHash = $"{rights.PrivateKey}_{actionForm.RuleId}_{actionForm.UserId}_{actionForm.PublicKey}_{rights.PrivateKey}";
            string hash = BitConverter.ToString(mD5.ComputeHash(Encoding.UTF8.GetBytes(rawHash))).Replace("-", "").ToUpper();

            if (hash != actionForm.Hash)
                throw new ForbiddenException("Invalid hash");
        }
    }
}