﻿using System.Web.Http;
using System.Web.Http.Cors;
using System.Threading.Tasks;
using System.Collections.Generic;

using Autofac;

using KarmaCounterServer.Dto;
using KarmaCounterServer.Model;
using KarmaCounterServer.Services;
using KarmaCounterServer.Exceptions;

namespace KarmaCounterServer.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UserController : ApiController
    {
        [HttpPost]
        public async Task<Session> Register([FromBody]RegistrationForm registrationForm)
        {
            if (!ModelState.IsValid || registrationForm == null || !registrationForm.IsValid)
                throw new BadRequestException(ModelState);

            return await Global.DI.Resolve<UserService>().Register(registrationForm);
        }

        [HttpPost]
        public async Task<Session> Login([FromBody]LoginForm loginForm)
        {
            if (!ModelState.IsValid || loginForm == null || !loginForm.IsValid)
                throw new BadRequestException(ModelState);

            return await Global.DI.Resolve<UserService>().Login(loginForm);
        }

        [HttpPost]
        public async Task<List<Invitation>> GetInvitations([FromBody]CheckedGetForm getForm)
        {
            if (!ModelState.IsValid || getForm == null || !getForm.IsValid)
                throw new BadRequestException(ModelState);

            return await Global.DI.Resolve<GroupInvitationService>().GetInvitations(getForm.Session);
        }

        [HttpPost]
        public async Task<Ownership> GetOwnership([FromBody] GetOwnershipForm getOwnershipForm)
        {
            if (!ModelState.IsValid || getOwnershipForm == null || !getOwnershipForm.IsValid)
                throw new BadRequestException(ModelState);

            return await Global.DI.Resolve<OwnershipService>().GetOwnership(getOwnershipForm);
        }

        [HttpPost]
        public async Task<List<Group>> GetOwnedGroups([FromBody] CheckedGetForm getForm)
        {
            if (!ModelState.IsValid || getForm == null || !getForm.IsValid)
                throw new BadRequestException(ModelState);

            return await Global.DI.Resolve<GroupService>().GetOwnedGroups(getForm);
        }

        [HttpGet]
        public async Task<List<Group>> GetOwnedGroups(long userId) =>
            (await Global.DI.Resolve<GroupService>().GetByOwnerId(userId)).owned;

        [HttpGet]
        public async Task<User> Get(long id) =>
            await Global.DI.Resolve<UserService>().GetUserById(id);

        [HttpGet]
        public async Task<User> Get(string login) =>
            await Global.DI.Resolve<UserService>().GetUserByLogin(login);

        [HttpGet]
        public async Task<List<RuleAction>> GetActions(long userId) =>
            await Global.DI.Resolve<UserService>().GetActions(userId);
    }
}
