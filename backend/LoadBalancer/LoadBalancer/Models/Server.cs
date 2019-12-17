using System;

using Newtonsoft.Json;

namespace LoadBalancer.Models
{
    [JsonObject]
    public class Server : IComparable
    {
        [JsonIgnore]
        public Uri Ip { get; private set; }

        [JsonProperty("domain")]
        public Uri Domain { get; private set; }

        [JsonProperty("session_count")]
        public int UserCount { get; private set; }

        public Server(string ip, string domain)
        {
            Ip = new Uri(ip);
            Domain = new Uri(domain);
        }

        public void Connect() => ++UserCount;
        public void Disconnect() => --UserCount;

        public int CompareTo(object obj) => UserCount - (obj as Server).UserCount;
    }
}