using System;
using System.Linq;
using System.Web.Http;
using System.Web.Hosting;
using System.Collections.Generic;
using System.Web.Http.Controllers;

using Logger;

using Autofac;

using IniParser;

using LoadBalancer.Util;
using LoadBalancer.Exceptions;

namespace LoadBalancer.Attributes
{
    public class WhitelistAttribute : AuthorizeAttribute
    {
        private ILogger logger = Global.DI.Resolve<ILogger>();
        private List<Uri> Whitelist { get; set; }

        public WhitelistAttribute(string[] urls) => Whitelist = urls.Select(U => new Uri(U)).ToList();
        public WhitelistAttribute(string whitelistIniFilePath, string sectionName, string keyName) =>
            Whitelist = new List<Uri>() { new Uri(new FileIniDataParser().ReadFile(HostingEnvironment.MapPath("~") + whitelistIniFilePath)[sectionName][keyName]) };

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            (Uri httpOrigin, Uri httpsOrigin) = RequestUtil.GetOriginIP(actionContext.Request);
            bool permit = Whitelist.Exists(A => A.Equals(httpOrigin) || A.Equals(httpsOrigin));

            logger.Trace($"Authorization {(permit ? "succeed" : "failed")} from {httpOrigin}");

            return permit;
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext) => 
            throw new WhitelistViolationException();
    }
}