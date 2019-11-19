using System;
using System.Web.Http;
using System.Threading.Tasks;

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
        private const string WHITELIST_INI_SECTION = "session_server";
        private const string WHITELIST_INI_KEY = "url";

        private Balancer balancer = Global.DI.Resolve<Balancer>();
        private ILogger logger = Global.DI.Resolve<ILogger>();

        [HttpGet]
        public Server Get() => balancer.GetServer();

        [HttpGet]
        [Whitelist(WHITELIST_INI_SECTION, WHITELIST_INI_KEY)]
        public async Task<BoolResult> Connect(string url) => await Auth(url, "connect", S => S.Connect());

        [HttpGet]
        [Whitelist(WHITELIST_INI_SECTION, WHITELIST_INI_KEY)]
        public async Task<BoolResult> Disconnect(string url) => await Auth(url, "disconnect", S => S.Disconnect());

        private async Task<BoolResult> Auth(string url, string action, Action<Server> callback)
        {
            if (!ModelState.IsValid)
                throw new BadRequestException(ModelState);

            Server localServer = balancer.GetByUrl(RequestUtil.GetUris(url));

            if (localServer == null)
            {
                await logger.Trace($"{action} to {url} failed: server not listed");
                throw new BadRequestException("Server not listed");
            }            

            callback(localServer);
            await logger.Trace($"{action} to {url} succeeded");

            return new BoolResult(true);
        }
    }
}