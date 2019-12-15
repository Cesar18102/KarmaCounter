using System;

using Newtonsoft.Json;

namespace LoadBalancer.Models
{
    [JsonObject]
    public class Server : IComparable
    {
        [JsonProperty("url")]
        public Uri Url { get; private set; }

        [JsonProperty("session_count")]
        public int UserCount { get; private set; }

        public Server(string url) => Url = new Uri(url);

        public void Connect() => ++UserCount;
        public void Disconnect() => --UserCount;

        public int CompareTo(object obj) => UserCount - (obj as Server).UserCount;
    }
}