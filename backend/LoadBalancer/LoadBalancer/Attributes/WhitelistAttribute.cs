using System;
using System.Net;
using System.Linq;
using System.Web.Http;
using System.Net.Http;
using System.Collections.Generic;
using System.Web.Http.Controllers;

using LoadBalancer.Util;
using LoadBalancer.Exceptions;

using Logger;
using Autofac;

namespace LoadBalancer.Attributes
{
    public class WhitelistAttribute : AuthorizeAttribute
    {
        private ILogger logger = Global.DI.Resolve<ILogger>();

        private List<Uri> Whitelist { get; set; }

        public WhitelistAttribute(params string[] urls) => Whitelist = urls.Select(U => new Uri(U)).ToList();

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            (Uri httpOrigin, Uri httpsOrigin) = RequestUtil.GetOriginIP(actionContext.Request);
            bool permit = Whitelist.Exists(A => A.Equals(httpOrigin) || A.Equals(httpsOrigin));

            logger.Debug($"Authorization {(permit ? "succeed" : "failed")} from {httpOrigin}");

            return permit;
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext) => throw new WhitelistViolationException();
    }
}