using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Autofac;

using Newtonsoft.Json;

using KarmaCounter.Models;
using KarmaCounter.ModelMapping;
using KarmaCounter.Server.Input;
using KarmaCounter.Server.Output;
using KarmaCounter.ResponseParser;
using KarmaCounter.Server.Interaction;
using KarmaCounter.ModelMapping.ModelMappingAttributes;

namespace KarmaCounter.Controllers
{
    public class GroupController
    {
        private static readonly LogicServerWrapper server = DI.Services.Resolve<LogicServerWrapper>();
        private static readonly IResponseParser parser = DI.Services.Resolve<IResponseParser>();
        private static readonly ModelMapper mapper = DI.Services.Resolve<ModelMapper>();

        private const string GET_ALL_GROUPS_SERVER_METHOD_NAME = "group/getall";
        public async Task<List<Group>> GetAll()
        {
            IQuery getAllGroupsQuery = new Query(QueryMethod.GET, GET_ALL_GROUPS_SERVER_METHOD_NAME);
            IServerResponse response = await server.SendQuery(getAllGroupsQuery, true);
            return parser.ParseCollection<Group, ResponseException>(response).ToList();
        }

        private const string GET_GROUP_INFO_SERVER_METHOD_NAME = "group/get";
        public async Task<Group> GetGroupDetailInfo(Group group)
        {
            IQuery getGroupInfoQuery = new Query(QueryMethod.GET, GET_GROUP_INFO_SERVER_METHOD_NAME, 
                parameters: await mapper.ExtractQueryParameters<Group, GetGroupById>(group));
            IServerResponse response = await server.SendQuery(getGroupInfoQuery, true);
            return parser.Parse<Group, ResponseException>(response);
        }

        private const string CREATE_GROUP_SERVER_METHOD_NAME = "group/create";
        public async Task<Ownership> Create(string name, string description, bool is_public, bool is_local)
        {
            Group group = new Group(name, description, is_public, is_local);

            IQuery createGroupQuery = new Query(QueryMethod.POST, CREATE_GROUP_SERVER_METHOD_NAME,
                await mapper.ExtractJsonQueryBody<Group, SessionWrapper, CreateGroup>(group, DI.Services.Resolve<SessionWrapper>()));
            IServerResponse response = await server.SendQuery(createGroupQuery, false);

            try
            {
                return response == null ? (await DI.Services.Resolve<UserController>().GetCurrentUserOwnedGroups()).LastOrDefault()?.Rights :
                                           parser.Parse<Ownership, ResponseException>(response);
            }
            catch (ResponseException ex)
            {
                PaymentRequiredException pex = null;

                try { pex = JsonConvert.DeserializeObject<PaymentRequiredException>(JsonConvert.SerializeObject(ex.body)); }
                catch { }

                throw (pex == null ? ex as Exception : pex);
            }
        }

        private const string ACCEPT_GROUP_CREATING_PAYMENT_SERVER_METHOD_NAME = "group/acceptpayment";
        public async Task<LiqpayPayment> CreateGroupCreatingPayment(string name, string description, bool is_public, bool is_local, double price)
        {
            Group group = new Group(name, description, is_public, is_local);
            return new LiqpayPayment(await mapper.ExtractJsonQueryBody<Group, SessionWrapper, CreateGroup>(group, DI.Services.Resolve<SessionWrapper>()), 
                                     group.Name, price, server.CurrentConnection + "/" + ACCEPT_GROUP_CREATING_PAYMENT_SERVER_METHOD_NAME);
        }

        private const string JOIN_GROUP_SERVER_METHOD_NAME = "group/join";
        public async Task<Membership> Join(Group group)
        {
            IQuery joinGroupQuery = new Query(QueryMethod.POST, JOIN_GROUP_SERVER_METHOD_NAME,
                await mapper.ExtractJsonQueryBody<Group, SessionWrapper, JoinGroup>(group, DI.Services.Resolve<SessionWrapper>()));
            IServerResponse response = await server.SendQuery(joinGroupQuery, false);
            return parser.Parse<Membership, ResponseException>(response);
        }

        private const string INVITE_GROUP_SERVER_METHOD_NAME = "group/invite";
        public async Task<Invitation> Invite(Group group, User user)
        {
            IQuery inviteGroupQuery = new Query(QueryMethod.POST, INVITE_GROUP_SERVER_METHOD_NAME,
                await mapper.ExtractJsonQueryBody<Group, User, SessionWrapper, InviteGroup>(group, user, DI.Services.Resolve<SessionWrapper>()));
            IServerResponse response = await server.SendQuery(inviteGroupQuery, false);
            return parser.Parse<Invitation, ResponseException>(response);
        }

        public const string ADD_RULE_SERVER_METHOD_NAME = "group/addrule";
        public async Task<Rule> AddRule(Rule rule, Group group)
        {
            IQuery addRuleQuery = new Query(QueryMethod.POST, ADD_RULE_SERVER_METHOD_NAME,
                await mapper.ExtractJsonQueryBody<Rule, Group, SessionWrapper, AddRuleAttribute>(rule, group, DI.Services.Resolve<SessionWrapper>()));
            IServerResponse response = await server.SendQuery(addRuleQuery, false);
            return parser.Parse<Rule, ResponseException>(response);
        }

        private const string GET_ACTIONS_GROUP_ID_PARAM_NAME = "groupId";
        private const string GET_ACTIONS_SERVER_METHOD_NAME = "group/getactionsbygroup";
        public async Task<List<RuleAction>> GetActions(long id)
        {
            IQuery getGroupActionsQuery = new Query(QueryMethod.GET, GET_ACTIONS_SERVER_METHOD_NAME,
                parameters: new Dictionary<string, string>() { { GET_ACTIONS_GROUP_ID_PARAM_NAME, id.ToString() } });
            IServerResponse response = await server.SendQuery(getGroupActionsQuery, true);
            return parser.ParseCollection<RuleAction, ResponseException>(response).ToList();
        }
    }
}
