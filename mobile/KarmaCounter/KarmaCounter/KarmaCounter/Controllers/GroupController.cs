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
using System;

namespace KarmaCounter.Controllers
{
    public class GroupController
    {
        private const string GET_ALL_GROUPS_SERVER_METHOD_NAME = "group/getall";
        public async Task<List<Group>> GetAll()
        {
            IQuery getAllGroupsQuery = new Query(QueryMethod.GET, GET_ALL_GROUPS_SERVER_METHOD_NAME);
            IServerResponse response = await DI.Services.Resolve<LogicServerWrapper>().SendQuery(getAllGroupsQuery, true);
            return DI.Services.Resolve<IResponseParser>().ParseCollection<Group, ResponseException>(response).ToList();
        }

        private const string GET_GROUP_INFO_SERVER_METHOD_NAME = "group/get";
        public async Task<Group> GetGroupDetailInfo(Group group)
        {
            IQuery getGroupInfoQuery = new Query(QueryMethod.GET, GET_GROUP_INFO_SERVER_METHOD_NAME, 
                parameters: await DI.Services.Resolve<ModelMapper>().ExtractQueryParameters<Group, GetGroupById>(group));
            IServerResponse response = await DI.Services.Resolve<LogicServerWrapper>().SendQuery(getGroupInfoQuery, true);
            return DI.Services.Resolve<IResponseParser>().Parse<Group, ResponseException>(response);
        }

        private const string CREATE_GROUP_SERVER_METHOD_NAME = "group/create";
        public async Task<Ownership> Create(string name, string description, bool is_public, bool is_local)
        {
            Group group = new Group(name, description, is_public, is_local);

            IQuery createGroupQuery = new Query(QueryMethod.POST, CREATE_GROUP_SERVER_METHOD_NAME,
                await DI.Services.Resolve<ModelMapper>().ExtractJsonQueryBody<Group, SessionWrapper, CreateGroup>(group, DI.Services.Resolve<SessionWrapper>()));
            IServerResponse response = await DI.Services.Resolve<LogicServerWrapper>().SendQuery(createGroupQuery, false);

            try
            {
                return response == null ? (await GetCurrentUserOwnedGroups()).LastOrDefault()?.Rights :
                                          DI.Services.Resolve<IResponseParser>().Parse<Ownership, ResponseException>(response);
            }
            catch (ResponseException ex)
            {
                PaymentRequiredException pex = null;

                try { pex = JsonConvert.DeserializeObject<PaymentRequiredException>(JsonConvert.SerializeObject(ex.body)); }
                catch { }

                throw (pex == null ? ex as Exception : pex);
            }
        }

        private const string GET_OWNERSHIP_SECURE_SERVER_METHOD_NAME = "user/getownership";
        public async Task<Ownership> GetOwnership(Group group)
        {
            IQuery getCurrentUserOwnership = new Query(QueryMethod.POST, GET_OWNERSHIP_SECURE_SERVER_METHOD_NAME,
                await DI.Services.Resolve<ModelMapper>().ExtractJsonQueryBody<Group, SessionWrapper, GetOwnership>(group, DI.Services.Resolve<SessionWrapper>()));
            IServerResponse response = await DI.Services.Resolve<LogicServerWrapper>().SendQuery(getCurrentUserOwnership, true);
            return DI.Services.Resolve<IResponseParser>().Parse<Ownership, ResponseException>(response);
        }

        private const string GET_OWNED_GROUPS_SECURE_SERVER_METHOD_NAME = "user/getownedgroups";
        public async Task<List<Group>> GetCurrentUserOwnedGroups()
        {
            IQuery getCurrentUserOwnedGroupsQuery = new Query(QueryMethod.POST, GET_OWNED_GROUPS_SECURE_SERVER_METHOD_NAME,
                JsonConvert.SerializeObject(DI.Services.Resolve<SessionWrapper>()));
            IServerResponse response = await DI.Services.Resolve<LogicServerWrapper>().SendQuery(getCurrentUserOwnedGroupsQuery, true);
            return DI.Services.Resolve<IResponseParser>().ParseCollection<Group, ResponseException>(response).ToList();
        }

        private const string ACCEPT_GROUP_CREATING_PAYMENT_SERVER_METHOD_NAME = "group/acceptpayment";
        public async Task<LiqpayPayment> CreateGroupCreatingPayment(string name, string description, bool is_public, bool is_local, double price)
        {
            Group group = new Group(name, description, is_public, is_local);

            string paymentInfo = await DI.Services.Resolve<ModelMapper>().ExtractJsonQueryBody<Group, SessionWrapper, CreateGroup>(group, 
                DI.Services.Resolve<SessionWrapper>());

            return new LiqpayPayment(paymentInfo, group.Name, price, 
                DI.Services.Resolve<LogicServerWrapper>().CurrentConnection + "/" + ACCEPT_GROUP_CREATING_PAYMENT_SERVER_METHOD_NAME);
        }

        private const string JOIN_GROUP_SERVER_METHOD_NAME = "group/join";
        public async Task<Membership> Join(Group group)
        {
            IQuery joinGroupQuery = new Query(QueryMethod.POST, JOIN_GROUP_SERVER_METHOD_NAME,
                await DI.Services.Resolve<ModelMapper>().ExtractJsonQueryBody<Group, SessionWrapper, JoinGroup>(group, DI.Services.Resolve<SessionWrapper>()));
            IServerResponse response = await DI.Services.Resolve<LogicServerWrapper>().SendQuery(joinGroupQuery, false);
            return DI.Services.Resolve<IResponseParser>().Parse<Membership, ResponseException>(response);
        }

        private const string INVITE_GROUP_SERVER_METHOD_NAME = "group/invite";
        public async Task<Invitation> Invite(Group group, User user)
        {
            IQuery inviteGroupQuery = new Query(QueryMethod.POST, INVITE_GROUP_SERVER_METHOD_NAME,
                await DI.Services.Resolve<ModelMapper>().ExtractJsonQueryBody<Group, User, SessionWrapper, InviteGroup>(group, user, DI.Services.Resolve<SessionWrapper>()));
            IServerResponse response = await DI.Services.Resolve<LogicServerWrapper>().SendQuery(inviteGroupQuery, false);
            return DI.Services.Resolve<IResponseParser>().Parse<Invitation, ResponseException>(response);
        }

        private const string USER_LOGIN_PARAM_NAME = "login";
        private const string GET_USER_BY_LOGIN_SERVER_METHOD_NAME = "user/get";
        public async Task<User> GetUserByLogin(string login)
        {
            IQuery getUserByLoginQuery = new Query(QueryMethod.GET, GET_USER_BY_LOGIN_SERVER_METHOD_NAME,
                parameters: new Dictionary<string, string>() { { USER_LOGIN_PARAM_NAME, login } });
            IServerResponse response = await DI.Services.Resolve<LogicServerWrapper>().SendQuery(getUserByLoginQuery, false);
            return DI.Services.Resolve<IResponseParser>().Parse<User, ResponseException>(response);
        }
    }
}
