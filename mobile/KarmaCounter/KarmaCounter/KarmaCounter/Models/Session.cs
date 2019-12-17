using Newtonsoft.Json;

namespace KarmaCounter.Models
{
    public class Session : IModelElement
    {
        [JsonProperty("userId")]
        public long UserId { get; private set; }

        [JsonProperty("token")]
        public string Token { get; private set; }

        public Session(long user_id, string token)
        {
            UserId = user_id;
            Token = token;
        }
    }
}
