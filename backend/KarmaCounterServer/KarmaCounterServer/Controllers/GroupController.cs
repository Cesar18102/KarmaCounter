using System.Web.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

using Autofac;

using KarmaCounterServer.Dto;
using KarmaCounterServer.Model;
using KarmaCounterServer.Services;
using KarmaCounterServer.Exceptions;

namespace KarmaCounterServer.Controllers
{
    public class GroupController : ApiController
    {
        [HttpPost]
        public async Task<Ownership> Create([FromBody] GroupForm groupForm)
        {
            if (!ModelState.IsValid || groupForm == null || !groupForm.IsValid)
                throw new BadRequestException(ModelState);

            return await Global.DI.Resolve<GroupService>().CreateGroup(groupForm);
        }

        [HttpPost]
        public async Task<Membership> Join([FromBody] JoinGroupForm joinForm)
        {
            if (!ModelState.IsValid || joinForm == null || !joinForm.IsValid)
                throw new BadRequestException(ModelState);

            return await Global.DI.Resolve<GroupService>().Join(joinForm);
        }

        [HttpPost]
        public async Task<Invitation> Invite([FromBody] InviteGroupForm inviteForm)
        {
            if (!ModelState.IsValid || inviteForm == null || !inviteForm.IsValid)
                throw new BadRequestException(ModelState);

            return await Global.DI.Resolve<GroupInvitationService>().Invite(inviteForm);
        }

        [HttpPost]
        public async Task<RuleAction> AccountRuleViolation([FromBody] RuleActionForm actionForm)
        {
            if (!ModelState.IsValid || actionForm == null || !actionForm.IsValid)
                throw new BadRequestException(ModelState);

            return await Global.DI.Resolve<RuleService>().AccountRuleViolation(actionForm);
        }

        [HttpPost]
        public async Task<RuleAction> AccountRuleAdherence([FromBody] RuleActionForm actionForm)
        {
            if (!ModelState.IsValid || actionForm == null || !actionForm.IsValid)
                throw new BadRequestException(ModelState);

            return await Global.DI.Resolve<RuleService>().AccountRuleAdherence(actionForm);
        }

        [HttpPost]
        public async Task<Rule> AddRule([FromBody] RuleForm ruleForm)
        {
            if (!ModelState.IsValid || ruleForm == null || !ruleForm.IsValid)
                throw new BadRequestException(ModelState);

            return await Global.DI.Resolve<RuleService>().AddRule(ruleForm);
        }

        [HttpPost]
        public async Task<Membership> UpdateCustomData([FromBody] SetCustomDataForm customDataForm)
        {
            if (!ModelState.IsValid || customDataForm == null || !customDataForm.IsValid)
                throw new BadRequestException(ModelState);

            return await Global.DI.Resolve<GroupService>().UpdateCustomData(customDataForm);
        }

        [HttpGet]
        public async Task<List<Group>> GetAll() =>
            await Global.DI.Resolve<GroupService>().GetAll();

        [HttpGet]
        public async Task<Group> Get(long id) =>
            await Global.DI.Resolve<GroupService>().GetById(id);

        [HttpGet]
        public async Task<List<RuleAction>> GetActionsByGroup(long groupId) =>
            await Global.DI.Resolve<GroupService>().GetActionsByGroup(groupId);

        [HttpGet]
        public async Task<List<RuleAction>> GetActionsByRule(long ruleId) =>
            await Global.DI.Resolve<GroupService>().GetActionsByRule(ruleId);
    }
}
