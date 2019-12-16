using System.Threading.Tasks;
using System.Collections.Generic;

using Autofac;

using KarmaCounterServer.Dto;
using KarmaCounterServer.Model;
using KarmaCounterServer.Exceptions;
using KarmaCounterServer.DataAccess;

namespace KarmaCounterServer.Services
{
    public class GroupInvitationService
    {
        public async Task<Invitation> Invite(InviteGroupForm inviteForm)
        {
            if (!(await Global.DI.Resolve<SessionService>().CheckSession(inviteForm.InviterSession)).Result) //if user is unauthorized
                throw new InvalidSessionException();

            User invitee = await Global.DI.Resolve<UserService>().GetUserById(inviteForm.InviteeId); //may throw not found exception
            Group sourceGroup = await Global.DI.Resolve<GroupService>().GetById(inviteForm.GroupId); //may throw not found exception

            if (sourceGroup.Members.Exists(M => M.Member.Id == invitee.Id)) //if user is alreary member of the group
                throw new ConflictException("Member");

            if (sourceGroup.Owner.Id == inviteForm.InviteeId)
                throw new ConflictException("Owner");

            if (!sourceGroup.Members.Exists(M => M.Member.Id == inviteForm.InviterSession.UserId) && sourceGroup.Owner.Id != inviteForm.InviterSession.UserId) 
                throw new ForbiddenException("You're not a member or creator of the group"); //if invitation is illegal e.g. inviter is not member or owner of the group

            return await Global.DI.Resolve<InvitationDataAccess>().Create(new Invitation(invitee, sourceGroup));
        }

        public async Task<List<Invitation>> GetInvitations(Session session)
        {
            if (!(await Global.DI.Resolve<SessionService>().CheckSession(session)).Result) //if user is unauthorized
                throw new InvalidSessionException();

            return await GetInvitations(session.UserId); //may throw not found exception
        }

        public async Task<bool> CheckInvitation(long userId, long groupId) =>
            (await GetInvitations(userId)).Exists(I => I.SourceGroup.Id == groupId);

        private async Task<List<Invitation>> GetInvitations(long userId)
        {
            User invitee = await Global.DI.Resolve<UserService>().GetUserById(userId); //may throw not found exception
            return await Global.DI.Resolve<InvitationDataAccess>().GetByUserId(invitee.Id);
        }
    }
}