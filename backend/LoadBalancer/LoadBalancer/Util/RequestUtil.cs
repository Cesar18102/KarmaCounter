using System;
using System.Web;
using System.Net.Http;

namespace LoadBalancer.Util
{
    public static class RequestUtil
    {
        public static (Uri http, Uri https) GetOriginIP(HttpRequestMessage requestMessage)
        {
            string origin = (requestMessage.Properties["MS_HttpContext"] as HttpContextWrapper).Request.UserHostAddress;

            if (origin == "::1")
                return (new Uri("http://127.0.0.1"), new Uri("https://127.0.0.1"));

            Uri httpOrigin = origin.Contains("http://") ? new Uri(origin) : new Uri("http://" + origin);
            Uri httpsOrigin = origin.Contains("https://") ? new Uri(origin) : new Uri("https://" + origin);

            return (httpOrigin, httpsOrigin);
        }
    }
}