using Newtonsoft.Json;

namespace KarmaCounter.Models
{
    public class ServerInfo : IModelElement
    {
        [JsonProperty("domain")]
        public string Url { get; private set; }

        [JsonProperty("session_count")]
        public int SessionsCount { get; private set; }

        public ServerInfo(string url, int session_count)
        {
            Url = url;
            SessionsCount = session_count;
        }
    }
}
