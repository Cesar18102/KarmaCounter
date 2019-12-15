using System;
using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

using SessionServer.Dto;

using SolidityEncoder;

namespace SessionServer.Model
{
    public class Session
    {
        private const string STUFF = "KARMA_COUNTER_SESSION_SERVER";
        private static KeccakEncoder Encoder = new KeccakEncoder();

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

        public Session(User user)
        {
            this.UserId = user.Id;
            this.Token = Encoder.Encode(
                new KeccakEncoder.ToBeHashed(KeccakEncoder.HashType.String, STUFF),
                new KeccakEncoder.ToBeHashed(KeccakEncoder.HashType.UInt256, DateTime.Now.Ticks.ToString()),
                new KeccakEncoder.ToBeHashed(KeccakEncoder.HashType.UInt256, UserId.ToString()),
                new KeccakEncoder.ToBeHashed(KeccakEncoder.HashType.String, STUFF)
            );
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Session))
                return false;

            Session s = obj as Session;
            return s.UserId == UserId && s.Token == Token;
        }
    }
}