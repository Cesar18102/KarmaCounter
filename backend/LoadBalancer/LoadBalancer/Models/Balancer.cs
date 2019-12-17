using System;
using System.Linq;
using System.Collections.Generic;

namespace LoadBalancer.Models
{
    public class Balancer
    {
        public Balancer(params string[] urls)
        {
            Servers = urls.Select(U =>
            {
                string[] ipDomain = U.Split('|');
                return new Server(ipDomain[0], ipDomain[1]);
            }).ToList();
        }

        public List<Server> Servers { get; set; }
        public Server GetServer() => Servers.Count == 0 ? null : Servers.Min();
        public Server GetByUrl((Uri http, Uri https) url) => Servers.SingleOrDefault(S => S.Ip.Equals(url.http) || S.Ip.Equals(url.https) ||
                                                                                          S.Domain.Equals(url.http) || S.Domain.Equals(url.https));
    }
}