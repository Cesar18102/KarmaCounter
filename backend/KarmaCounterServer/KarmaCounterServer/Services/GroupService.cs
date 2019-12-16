using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Cryptography;

using SolidityEncoder;

using Autofac;

using KarmaCounterServer.Dto;
using KarmaCounterServer.Model;
using KarmaCounterServer.DataAccess;
using KarmaCounterServer.Exceptions;
using KarmaCounterServer.ModelMapping.AttributeTemplates.Group;

namespace KarmaCounterServer.Services
{
    public class GroupService
    {
        private const int MAX_FREE_GROUP_CREATIONS = 1;
        private const double GLOBAL_GROUP_CREATION_MIN_FEE = 10;

        private double CalcExtraGroupCreationFee(int count) =>
            count - MAX_FREE_GROUP_CREATIONS;

        private double CalcGlobalGroupCreationFee(int count) =>
            GLOBAL_GROUP_CREATION_MIN_FEE + count - MAX_FREE_GROUP_CREATIONS;

        public async Task<Ownership> CreateGroup(GroupForm groupForm, bool paid = false)
        {
            KeccakEncoder encoder = new KeccakEncoder();
            string publicKey = encoder.Encode(new KeccakEncoder.ToBeHashed(KeccakEncoder.HashType.String, Guid.NewGuid().ToString()));
            string privateKey = encoder.Encode(new KeccakEncoder.ToBeHashed(KeccakEncoder.HashType.String, Guid.NewGuid().ToString()));

            if (!(await Global.DI.Resolve<SessionService>().CheckSession(groupForm.CreatorSession)).Result) //if user is unauthorized
                throw new InvalidSessionException();

            (User owner, List<Group> owned) owningInfo = await GetByOwnerId(groupForm.CreatorSession.UserId); //may throw not found exception

            if (!paid && owningInfo.owned.Count >= MAX_FREE_GROUP_CREATIONS) //ask for payment if some groups already created
                throw new PaymentNeededException(CalcExtraGroupCreationFee(owningInfo.owned.Count + 1));

            if (!paid && !groupForm.IsLocal) //ask for payment if creating global group
                throw new PaymentNeededException(CalcGlobalGroupCreationFee(owningInfo.owned.Count + 1));

            Ownership ownership = new Ownership(owningInfo.owner, publicKey, privateKey);
            Group group = new Group(groupForm.Name, groupForm.Description, groupForm.IsPublic, groupForm.IsLocal, ownership);

            return (await Global.DI.Resolve<GroupDataAccess>().Create(group)).Rights;
        }

        public async Task<Membership> Join(JoinGroupForm joinForm)
        {
            if (!(await Global.DI.Resolve<SessionService>().CheckSession(joinForm.MemberSession)).Result) //if user is unauthorized
                throw new InvalidSessionException();

            User member = await Global.DI.Resolve<UserService>().GetUserById(joinForm.MemberSession.UserId); //may throw not found exception
            Group group = await GetById(joinForm.GroupId); //may throw not found exception

            if (group.Members.Exists(M => M.Member.Id == joinForm.MemberSession.UserId)) //if user is already a member of the group
                throw new ConflictException("Member");

            if (group.Owner.Id == joinForm.MemberSession.UserId) //if user is an owner of the group
                throw new ConflictException("Owner");

            if (!group.IsPublic && !(await Global.DI.Resolve<GroupInvitationService>().CheckInvitation(member.Id, group.Id)))
                throw new NotFoundException("Invitation"); //if group is not public and user not invited

            Membership membership = new Membership(group, member);
            return await Global.DI.Resolve<MembershipDataAccess>().Create(membership);
        }

        public async Task<Group> GetById(long id)
        {
            Group group = await Global.DI.Resolve<GroupDataAccess>().GetById<GroupSelect>(id);

            if (group == null)
                throw new NotFoundException("Group");

            return group;
        }

        public async Task<(User owner, List<Group> owned)> GetByOwnerId(long userId)
        {
            User owner = await Global.DI.Resolve<UserService>().GetUserById(userId); //may throw not found exception
            return (owner, await Global.DI.Resolve<GroupDataAccess>().GetByOwner<GroupSelect>(owner));
        }

        public async Task<List<Group>> GetOwnedGroups(CheckedGetForm checkedGetForm)
        {
            if (!(await Global.DI.Resolve<SessionService>().CheckSession(checkedGetForm.Session)).Result) //if user is unauthorized
                throw new InvalidSessionException();

            User owner = await Global.DI.Resolve<UserService>().GetUserById(checkedGetForm.Session.UserId); //may throw not found exception
            return await Global.DI.Resolve<GroupDataAccess>().GetByOwner<GroupSelectSecure>(owner);
        }

        public async Task<Group> GetByPublicKey(string public_key)
        {
            Group group = await Global.DI.Resolve<GroupDataAccess>().GetByPublicKey(public_key);

            if (group == null)
                throw new NotFoundException("Public key");

            return group;
        }

        public async Task<List<Group>> GetAll() =>
            await Global.DI.Resolve<GroupDataAccess>().GetAll();

        public async Task<List<RuleAction>> GetActionsByGroup(long groupId)
        {
            Group group = await GetById(groupId); //may throw not found exception
            return await Global.DI.Resolve<RuleActionDataAccess>().GetByGroup(group);
        }

        public async Task<List<RuleAction>> GetActionsByRule(long ruleId)
        {
            Rule rule = await Global.DI.Resolve<RuleService>().GetById(ruleId); //may throw not found exception
            return await Global.DI.Resolve<RuleActionDataAccess>().GetByRule(rule);
        }

        public async Task<Membership> UpdateCustomData(SetCustomDataForm customDataForm)
        {
            Group group = await Global.DI.Resolve<GroupService>().GetByPublicKey(customDataForm.PublicKey); //may throw not found exception
            CheckKeys(customDataForm, group.Rights); //may throw forbidden exception

            Membership membership = group.Members.SingleOrDefault(M => M.Member.Id == customDataForm.UserId);

            if (membership == null)
                throw new NotFoundException("Membership");

            return await Global.DI.Resolve<MembershipDataAccess>().Update(new Membership(new Membership(membership, group), customDataForm.CustomData));
        }

        private static readonly MD5 mD5 = MD5.Create();
        private void CheckKeys(SetCustomDataForm customDataForm, Ownership rights)
        {
            string rawHash = $"{rights.PrivateKey}_{customDataForm.UserId}_{customDataForm.GroupId}_{customDataForm.PublicKey}_{customDataForm.CustomData}_{rights.PrivateKey}";
            string hash = BitConverter.ToString(mD5.ComputeHash(Encoding.UTF8.GetBytes(rawHash))).Replace("-", "").ToUpper();

            if (hash != customDataForm.Hash)
                throw new ForbiddenException("Invalid hash");
        }
    }
}