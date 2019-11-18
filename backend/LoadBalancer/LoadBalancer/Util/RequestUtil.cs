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

            return GetUris(origin);
        }

        public static (Uri http, Uri https) GetUris(string url)
        {
            string domain = GetDomain(url);
            return (new Uri("http://" + domain), new Uri("https://" + domain));
        }

        public static Uri GetHttpUri(string url) => new Uri("http://" + GetDomain(url));
        public static Uri GetHttpsUri(string url) => new Uri("http://" + GetDomain(url));

        public static string GetDomain(string url) => url.Replace("http://", "").Replace("https://", "");
    }
}