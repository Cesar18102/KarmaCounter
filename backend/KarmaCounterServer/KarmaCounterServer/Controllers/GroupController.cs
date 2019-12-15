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

        [HttpGet]
        public async Task<List<Group>> GetByOwnerId(long id) =>
            (await Global.DI.Resolve<GroupService>().GetByOwnerId(id)).owned;

        [HttpGet]
        public async Task<List<Group>> GetAll() =>
            await Global.DI.Resolve<GroupService>().GetAll();

        [HttpGet]
        public async Task<Group> Get(long id) =>
            await Global.DI.Resolve<GroupService>().GetById(id);
    }
}
