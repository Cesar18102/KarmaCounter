using System;
using System.Threading.Tasks;

using SolidityEncoder;

using Autofac;

using KarmaCounterServer.Dto;
using KarmaCounterServer.Model;
using KarmaCounterServer.DataAccess;
using KarmaCounterServer.Exceptions;

namespace KarmaCounterServer.Services
{
    public class GroupService
    {
        public async Task<Ownership> CreateGroup(GroupForm groupForm)
        {
            KeccakEncoder encoder = new KeccakEncoder();
            string publicKey = encoder.Encode(new KeccakEncoder.ToBeHashed(KeccakEncoder.HashType.String, Guid.NewGuid().ToString()));
            string privateKey = encoder.Encode(new KeccakEncoder.ToBeHashed(KeccakEncoder.HashType.String, Guid.NewGuid().ToString()));

            User owner = new User(groupForm.CreatorSession.UserId);

            //check if user exists
            //check if session alive
            //check count of created groups

            Ownership ownership = new Ownership(owner, publicKey, privateKey);
            Group group = new Group(groupForm.Name, groupForm.Description, groupForm.IsPublic, groupForm.IsLocal, ownership);

            return (await Global.DI.Resolve<GroupDataAccess>().Create(group)).Rights;
        }

        public async Task<Membership> Join(JoinGroupForm joinForm)
        {
            //check user session
            //check if user already a member of this group

            User user = await Global.DI.Resolve<UserService>().GetUserById(joinForm.MemberSession.UserId); //may throw not found exception
            Group group = await GetById(joinForm.GroupId); //may throw not found exception

            Membership membership = new Membership(group, user);
            return await Global.DI.Resolve<MembershipDataAccess>().Create(membership);
        }

        public async Task<Group> GetById(long id)
        {
            Group group = await Global.DI.Resolve<GroupDataAccess>().GetById(id);

            if (group == null)
                throw new NotFoundException("Group");

            return group;
        }
    }
}