using System;
using System.Linq;
using System.Collections.Generic;

namespace LoadBalancer.Models
{
    public class Balancer
    {
        public Balancer(params string[] serverUrls) => Servers = serverUrls.Select(SU => new Server(SU)).ToList();

        public List<Server> Servers { get; set; }
        public Server GetServer() => Servers.Count == 0 ? null : Servers.Min();
        public Server GetByUrl((Uri http, Uri https) url) => Servers.SingleOrDefault(S => S.Url.Equals(url.http) || S.Url.Equals(url.https));
    }
}