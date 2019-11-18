using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using Autofac;

using Logger;
using LoadBalancer.Util;
using LoadBalancer.Models;
using LoadBalancer.Attributes;
using LoadBalancer.Exceptions;

namespace LoadBalancer.Controllers
{
    public class ServerController : ApiController
    {
        private const string SESSION_SERVER = "http://109.86.209.135";

        private Balancer balancer = Global.DI.Resolve<Balancer>();
        private ILogger logger = Global.DI.Resolve<ILogger>();

        [HttpGet]
        public Server Get() => balancer.GetServer();

        [HttpGet]
        [Whitelist(SESSION_SERVER)]
        public BoolResult Connect(string url) => Auth(url, "connect", S => S.Connect());

        [HttpGet]
        [Whitelist(SESSION_SERVER)]
        public BoolResult Disconnect(string url) => Auth(url, "disconnect", S => S.Disconnect());

        private BoolResult Auth(string url, string action, Action<Server> callback)
        {
            if (!ModelState.IsValid)
                throw new BadRequestException(ModelState);

            logger.Debug($"{action} to {url} succeeded");
            Server localServer = balancer.GetByUrl(RequestUtil.GetUris(url));

            if (localServer == null)
            {
                logger.Debug($"{action} to {url} failed: server not listed");
                throw new BadRequestException("Server not listed");
            }

            callback(localServer);

            return new BoolResult(true);
        }
    }
}