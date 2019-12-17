using System.Threading.Tasks;

using Autofac;

using KarmaCounter.Models;
using KarmaCounter.Server.Input;
using KarmaCounter.Server.Output;
using KarmaCounter.ResponseParser;
using KarmaCounter.Server.Interaction;
using Newtonsoft.Json;

namespace KarmaCounter.Controllers
{
    public class AuthController
    {
        private const string SIGN_UP_SERVER_METHOD_NAME = "user/register";
        public async Task<Session> SignUp(string login, string password, string email)
        {
            SignUpForm signUpForm = new SignUpForm(login, password, email);
            IQuery signUpQuery = new Query(QueryMethod.POST, SIGN_UP_SERVER_METHOD_NAME, JsonConvert.SerializeObject(signUpForm));
            IServerResponse response = await DI.Services.Resolve<LogicServerWrapper>().SendQuery(signUpQuery, false);

            Session session = response == null ? await LogIn(login, password) : 
                DI.Services.Resolve<IResponseParser>().Parse<Session, ResponseException>(response);

            DI.Services.Resolve<SessionWrapper>().CurrentUserSession = session;
            return session;
        }

        private const string LOG_IN_SERVER_METHOD_NAME = "user/login";
        public async Task<Session> LogIn(string login, string password)
        {
            LogInForm logInForm = new LogInForm(login, password);
            IQuery logInQuery = new Query(QueryMethod.POST, LOG_IN_SERVER_METHOD_NAME, JsonConvert.SerializeObject(logInForm));
            IServerResponse response = await DI.Services.Resolve<LogicServerWrapper>().SendQuery(logInQuery, true);

            Session session = DI.Services.Resolve<IResponseParser>().Parse<Session, ResponseException>(response);
            DI.Services.Resolve<SessionWrapper>().CurrentUserSession = session;
            return session;
        }
    }
}
