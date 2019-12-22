using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Autofac;

using Newtonsoft.Json;

using KarmaCounter.Models;
using KarmaCounter.Server.Input;
using KarmaCounter.ModelMapping;
using KarmaCounter.Server.Output;
using KarmaCounter.ResponseParser;
using KarmaCounter.Server.Interaction;
using KarmaCounter.ModelMapping.ModelMappingAttributes;

namespace KarmaCounter.Controllers
{
    public class UserController
    {
        private static readonly LogicServerWrapper server = DI.Services.Resolve<LogicServerWrapper>();
        private static readonly IResponseParser parser = DI.Services.Resolve<IResponseParser>();
        private static readonly ModelMapper mapper = DI.Services.Resolve<ModelMapper>();

        private const string GET_OWNERSHIP_SECURE_SERVER_METHOD_NAME = "user/getownership";
        public async Task<Ownership> GetOwnership(Group group)
        {
            IQuery getCurrentUserOwnership = new Query(QueryMethod.POST, GET_OWNERSHIP_SECURE_SERVER_METHOD_NAME,
                await mapper.ExtractJsonQueryBody<Group, SessionWrapper, GetOwnership>(group, DI.Services.Resolve<SessionWrapper>()));
            IServerResponse response = await server.SendQuery(getCurrentUserOwnership, true);
            return parser.Parse<Ownership, ResponseException>(response);
        }

        private const string GET_OWNED_GROUPS_SECURE_SERVER_METHOD_NAME = "user/getownedgroups";
        public async Task<List<Group>> GetCurrentUserOwnedGroups()
        {
            IQuery getCurrentUserOwnedGroupsQuery = new Query(QueryMethod.POST, GET_OWNED_GROUPS_SECURE_SERVER_METHOD_NAME,
                JsonConvert.SerializeObject(DI.Services.Resolve<SessionWrapper>()));
            IServerResponse response = await server.SendQuery(getCurrentUserOwnedGroupsQuery, true);
            return parser.ParseCollection<Group, ResponseException>(response).ToList();
        }

        private const string USER_LOGIN_PARAM_NAME = "login";
        private const string GET_USER_BY_LOGIN_SERVER_METHOD_NAME = "user/get";
        public async Task<User> GetUserByLogin(string login)
        {
            IQuery getUserByLoginQuery = new Query(QueryMethod.GET, GET_USER_BY_LOGIN_SERVER_METHOD_NAME,
                parameters: new Dictionary<string, string>() { { USER_LOGIN_PARAM_NAME, login } });
            IServerResponse response = await server.SendQuery(getUserByLoginQuery, true);
            return parser.Parse<User, ResponseException>(response);
        }

        private const string USER_ID_PARAM_NAME = "id";
        private const string GET_USER_BY_ID_SERVER_METHOD_NAME = "user/get";
        public async Task<User> GetUserById(long id)
        {
            IQuery getUserByIdQuery = new Query(QueryMethod.GET, GET_USER_BY_ID_SERVER_METHOD_NAME,
                parameters: new Dictionary<string, string>() { { USER_ID_PARAM_NAME, id.ToString() } });
            IServerResponse response = await server.SendQuery(getUserByIdQuery, true);
            return parser.Parse<User, ResponseException>(response);
        }

        private const string GET_ACTIONS_USER_ID_PARAM_NAME = "userId";
        private const string GET_ACTIONS_SERVER_METHOD_NAME = "user/getactions";
        public async Task<List<RuleAction>> GetActions(long id)
        {
            IQuery getUserActionsQuery = new Query(QueryMethod.GET, GET_ACTIONS_SERVER_METHOD_NAME,
                parameters: new Dictionary<string, string>() { { GET_ACTIONS_USER_ID_PARAM_NAME, id.ToString() } });
            IServerResponse response = await server.SendQuery(getUserActionsQuery, true);
            return parser.ParseCollection<RuleAction, ResponseException>(response).ToList();
        }
    }
}
