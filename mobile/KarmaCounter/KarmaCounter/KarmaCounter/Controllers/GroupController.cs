﻿using System.Linq;
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
    }
}
