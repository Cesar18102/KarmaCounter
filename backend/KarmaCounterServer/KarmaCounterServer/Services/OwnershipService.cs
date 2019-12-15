using System.Threading.Tasks;
using System.Collections.Generic;

using Autofac;

using KarmaCounterServer.Dto;
using KarmaCounterServer.Model;
using KarmaCounterServer.Exceptions;
using KarmaCounterServer.DataAccess;

namespace KarmaCounterServer.Services
{
    public class OwnershipService
    {
        public async Task<Ownership> GetOwnership(GetOwnershipForm getOwnershipForm)
        {
            if (!(await Global.DI.Resolve<SessionService>().CheckSession(getOwnershipForm.CreatorSession)).Result) //if user is unauthorized
                throw new InvalidSessionException();

            Ownership ownership = await Global.DI.Resolve<OwnershipDataAccess>().GetByGroupId(getOwnershipForm.GroupId);

            if (ownership == null)
                throw new NotFoundException("Group");

            if (ownership.Owner.Id != getOwnershipForm.CreatorSession.UserId)
                throw new ForbiddenException("You aren't group owner");

            return ownership;
        }

        public async Task<List<Ownership>> GetUserOwnerships(CheckedGetForm checkedGetForm)
        {
            if (!(await Global.DI.Resolve<SessionService>().CheckSession(checkedGetForm.Session)).Result) //if user is unauthorized
                throw new InvalidSessionException();

            return await Global.DI.Resolve<OwnershipDataAccess>().GetByOwnerUserId(checkedGetForm.Session.UserId);
        }
    }
}