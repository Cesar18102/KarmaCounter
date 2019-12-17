using System;
using System.IO;
using System.Net;
using System.Web.Http;
using System.Collections.Generic;

using Newtonsoft.Json;

using Autofac;

using SessionServer.Dto;
using SessionServer.Util;
using SessionServer.Model;
using SessionServer.Attributes;
using SessionServer.Exceptions;

using Logger;

namespace SessionServer.Controllers
{
    public class SessionController : ApiController
    {
        private const string WHITELIST_INI_FILE_NAME = "bin/whitelist.ini";
        private const string WHITELIST_INI_SECTION = "servers";

        private const string LOAD_BALANCER_URL = "http://karmaloadbalancer.somee.com/";
        private const string LOAD_BALANCER_CONNECT = LOAD_BALANCER_URL + "api/server/connect";
        private const string LOAD_BALANCER_DISCONNECT = LOAD_BALANCER_URL + "api/server/disconnect";

        private BoolResult SendLoadBalancer(bool connect, string url)
        {
            HttpWebRequest request = HttpWebRequest.CreateHttp($"{(connect ? LOAD_BALANCER_CONNECT : LOAD_BALANCER_DISCONNECT)}?url={url}");
            request.Method = WebRequestMethods.Http.Get;

            string responseText = "";
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            using (StreamReader str = new StreamReader(response.GetResponseStream()))
                responseText = str.ReadToEnd();

            try { return JsonConvert.DeserializeObject<BoolResult>(responseText); }
            catch { throw JsonConvert.DeserializeObject<BadRequestException>(responseText); }
        }

        [HttpPost]
        [Whitelist(WHITELIST_INI_FILE_NAME, WHITELIST_INI_SECTION)]
        public Session Create([FromBody]User user)
        {
            if (!ModelState.IsValid)
                throw new BadRequestException(ModelState);

            (Uri http, Uri https) = RequestUtil.GetOriginIP(Request);
            if (!SendLoadBalancer(true, http.ToString()).Result && !SendLoadBalancer(true, https.ToString()).Result)
                throw new WhitelistViolationException();

            Global.DI.Resolve<ILogger>().Trace($"Session of user {user.Id} was created");
            return Global.DI.Resolve<SessionTable>().Create(user, http.ToString().Replace("http://", ""));
        }

        [HttpPost]
        [Whitelist(WHITELIST_INI_FILE_NAME, WHITELIST_INI_SECTION)]
        public void Terminate([FromBody]Session s)
        {
            if (!ModelState.IsValid)
                throw new BadRequestException(ModelState);

            if (!Global.DI.Resolve<SessionTable>().Terminate(s))
                throw new BadRequestException("Session doesn't exists");

            (Uri http, Uri https) = RequestUtil.GetOriginIP(Request);
            if (!SendLoadBalancer(true, http.ToString()).Result && !SendLoadBalancer(false, https.ToString()).Result)
                throw new WhitelistViolationException();

            Global.DI.Resolve<ILogger>().Trace($"Session of user {s.UserId} was terminated");
        }

        [HttpPost]
        [Whitelist(WHITELIST_INI_FILE_NAME, WHITELIST_INI_SECTION)]
        public BoolResult Check([FromBody] Session session)
        {
            if (!ModelState.IsValid)
                throw new BadRequestException(ModelState);

            return new BoolResult(Global.DI.Resolve<SessionTable>().IsSessionAlive(session));
        }

        public Dictionary<string, List<Session>> GetAll() =>
            Global.DI.Resolve<SessionTable>().Sessions;
    }
}
