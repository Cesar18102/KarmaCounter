using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace KarmaCounterServer.Model
{
    public class Session
    {
        [Required]
        [JsonRequired]
        [JsonProperty("userId")]
        public long UserId { get; private set; }

        [Required]
        [JsonRequired]
        [JsonProperty("token")]
        public string Token { get; private set; }

        [JsonConstructor]
        public Session(long userId, string token)
        {
            UserId = userId;
            Token = token;
        }
    }
}