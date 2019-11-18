using System;
using System.Net;
using System.Web.Http;

using Autofac;

using LoadBalancer.Log;
using LoadBalancer.Util;
using LoadBalancer.Models;
using System.Net.Http;

namespace LoadBalancer.Controllers
{
    public class ServerController : ApiController
    {
        [HttpGet]
        public Server Get() => Global.DI.Resolve<Balancer>().GetServer();

        [HttpGet]
        public BoolResult Connect()
        {
            Server server = CheckSender(Request);

            if (server == null)
            {
                Global.DI.Resolve<ILogger>().Debug($"Failed to log in");
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }

            Global.DI.Resolve<ILogger>().Debug($"Logged in from {server.Url}");
            server.Connect();

            return new BoolResult(true);
        }

        [HttpGet]
        public BoolResult Disconnect()
        {
            Server server = CheckSender(Request);

            if (server == null)
            {
                Global.DI.Resolve<ILogger>().Debug($"Failed to log out");
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }

            Global.DI.Resolve<ILogger>().Debug($"Logged out from {server.Url}");
            server.Disconnect();

            return new BoolResult(true);
        }

        private Server CheckSender(HttpRequestMessage request)
        {
            (Uri httpOrigin, Uri httpsOrigin) = RequestUtil.GetOriginIP(request);
            return Global.DI.Resolve<Balancer>().GetByUrl(httpOrigin, httpsOrigin);
        }
    }
}