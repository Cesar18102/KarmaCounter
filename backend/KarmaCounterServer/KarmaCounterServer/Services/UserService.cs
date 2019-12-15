using System.Threading.Tasks;

using Autofac;

using SolidityEncoder;

using KarmaCounterServer.Dto;
using KarmaCounterServer.Model;
using KarmaCounterServer.Exceptions;
using KarmaCounterServer.DataAccess;

namespace KarmaCounterServer.Services
{
    public class UserService
    {
        public async Task<Session> Register(RegistrationForm registrationForm)
        {
            KeccakEncoder encoder = new KeccakEncoder();
            string encodedPassword = encoder.Encode(new KeccakEncoder.ToBeHashed(KeccakEncoder.HashType.String, registrationForm.Password));

            User user = new User(registrationForm.Login, encodedPassword, registrationForm.Email);

            if (await Global.DI.Resolve<UserDataAccess>().GetByLogin(user.Login) != null)
                throw new ConflictException("User");

            await Global.DI.Resolve<UserDataAccess>().Create(user);
            User createdUser = await GetUserByLogin(user.Login);
            return await Global.DI.Resolve<SessionService>().CreateSession(createdUser.Id.ToString());
        }

        public async Task<Session> Login(LoginForm loginForm)
        {
            User user = await GetUserByLogin(loginForm.Login); //may throw not found exception

            if (!(await Global.DI.Resolve<UserDataAccess>().CheckPassword(loginForm)))
                throw new WrongPasswordException();

            return await Global.DI.Resolve<SessionService>().CreateSession(user.Id.ToString());
        }

        public async Task<User> GetUserByLogin(string login)
        {
            User user = await Global.DI.Resolve<UserDataAccess>().GetByLogin(login);

            if(user == null)
                throw new NotFoundException("User");

            return user;
        }

        public async Task<User> GetUserById(long id)
        {
            User user = await Global.DI.Resolve<UserDataAccess>().GetById(id);

            if (user == null)
                throw new NotFoundException("User");

            return user;
        }
    }
}