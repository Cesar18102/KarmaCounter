using System.Web.Http;
using System.Threading.Tasks;

using Autofac;

using KarmaCounterServer.Dto;
using KarmaCounterServer.Model;
using KarmaCounterServer.Services;
using KarmaCounterServer.Exceptions;

namespace KarmaCounterServer.Controllers
{
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

        [HttpGet]
        public async Task<User> Get(long id) =>
            await Global.DI.Resolve<UserService>().GetUserById(id);

        [HttpGet]
        public async Task<User> Get(string login) =>
            await Global.DI.Resolve<UserService>().GetUserByLogin(login);

    }
}
