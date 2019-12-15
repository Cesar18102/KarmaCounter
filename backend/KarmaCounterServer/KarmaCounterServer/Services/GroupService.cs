﻿using System;
using System.Threading.Tasks;
using System.Collections.Generic;

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

            if (!(await Global.DI.Resolve<SessionService>().CheckSession(groupForm.CreatorSession)).Result) //if user is unauthorized
                throw new InvalidSessionException();

            (User owner, List<Group> owned) owningInfo = await GetByOwnerId(groupForm.CreatorSession.UserId); //may throw not found exception

            if (owningInfo.owned.Count >= 1) //ask for payment if some groups already created
                throw new PaymentNeededException(owningInfo.owned.Count - 1);

            Ownership ownership = new Ownership(owningInfo.owner, publicKey, privateKey);
            Group group = new Group(groupForm.Name, groupForm.Description, groupForm.IsPublic, groupForm.IsLocal, ownership);

            return (await Global.DI.Resolve<GroupDataAccess>().Create(group)).Rights;
        }

        public async Task<Membership> Join(JoinGroupForm joinForm)
        {
            if (!(await Global.DI.Resolve<SessionService>().CheckSession(joinForm.MemberSession)).Result) //if user is unauthorized
                throw new InvalidSessionException();

            User member = await Global.DI.Resolve<UserService>().GetUserById(joinForm.MemberSession.UserId); //may throw not found exception
            Group group = await Global.DI.Resolve<GroupDataAccess>().GetById(joinForm.GroupId); //may throw not found exception

            if (group.Members.Exists(M => M.Member.Id == joinForm.MemberSession.UserId)) //if user is already a member of the group
                throw new ConflictException("Member");

            if (group.Owner.Id == joinForm.MemberSession.UserId) //if user is an owner of the group
                throw new ConflictException("Owner");

            Membership membership = new Membership(group, member);
            return await Global.DI.Resolve<MembershipDataAccess>().Create(membership);
        }

        public async Task<(User owner, List<Group> owned)> GetByOwnerId(long userId)
        {
            User owner = await Global.DI.Resolve<UserService>().GetUserById(userId); //may throw not found exception
            return (owner, await Global.DI.Resolve<GroupDataAccess>().GetByOwner(owner));
        }

        public async Task<List<Group>> GetAll() =>
            await Global.DI.Resolve<GroupDataAccess>().GetAll();

        public async Task<Group> GetById(long id)
        {
            Group group = await Global.DI.Resolve<GroupDataAccess>().GetById(id);

            if (group == null)
                throw new NotFoundException("Group");

            return group;
        }
    }
}