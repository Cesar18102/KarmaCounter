using System;
using System.Linq;
using System.Collections.Generic;

namespace LoadBalancer.Models
{
    public class Balancer
    {
        private List<Server> Servers = new List<Server>()
        {
            new Server("http://37.229.135.155"),
            new Server("http://localhost"),
            new Server("http://127.0.0.1")
        };

        public Server GetServer() => Servers.Count == 0 ? null : Servers.Min();

        public bool Contains(Uri httpUrl, Uri httpsUrl) => GetByUrl(httpUrl, httpsUrl) != null;
        public Server GetByUrl(Uri httpUrl, Uri httpsUrl) => Servers.SingleOrDefault(S => S.Url.Equals(httpUrl) || S.Url.Equals(httpsUrl));
    }
}