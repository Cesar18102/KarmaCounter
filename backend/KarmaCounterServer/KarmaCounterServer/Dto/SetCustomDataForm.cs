using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace KarmaCounterServer.Dto
{
    public class SetCustomDataForm : DtoForm
    {
        [Required]
        [JsonRequired]
        [JsonProperty("user_id")]
        public long UserId { get; private set; }

        [Required]
        [JsonRequired]
        [JsonProperty("group_id")]
        public long GroupId { get; private set; }

        [Required]
        [JsonRequired]
        [JsonProperty("custom_data")]
        public string CustomData { get; private set; }

        [Required]
        [JsonRequired]
        [JsonProperty("public_key")]
        public string PublicKey { get; private set; }

        [Required]
        [JsonRequired]
        [JsonProperty("hash")]
        public string Hash { get; private set; }

        public SetCustomDataForm(long user_id, long group_id, string custom_data, string public_key, string hash)
        {
            UserId = user_id;
            GroupId = group_id;
            CustomData = custom_data;
            PublicKey = public_key;
            Hash = hash;
        }

        public override bool IsValid => true;
    }
}