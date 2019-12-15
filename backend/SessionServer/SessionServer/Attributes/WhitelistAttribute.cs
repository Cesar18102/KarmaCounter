using System;
using System.Linq;
using System.Web.Http;
using System.Web.Hosting;
using System.Collections.Generic;
using System.Web.Http.Controllers;

using Autofac;

using IniParser;
using IniParser.Model;

using Logger;

using SessionServer.Util;
using SessionServer.Exceptions;

namespace SessionServer.Attributes
{
    public class WhitelistAttribute : AuthorizeAttribute
    {
        private ILogger logger = Global.DI.Resolve<ILogger>();
        private List<Uri> Whitelist { get; set; }

        public WhitelistAttribute(string[] urls) => Whitelist = urls.Select(U => new Uri(U)).ToList();
        public WhitelistAttribute(string whitelistIniFilePath, string sectionName)
        {
            Whitelist = new List<Uri>();
            KeyDataCollection datas = new FileIniDataParser().ReadFile(HostingEnvironment.MapPath("~") + whitelistIniFilePath)[sectionName];

            foreach (KeyData data in datas)
                Whitelist.Add(new Uri(data.Value));
        }

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