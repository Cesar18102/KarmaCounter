using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace KarmaCounterServer.Dto
{
    public class RuleActionForm : DtoForm
    {
        [Required]
        [JsonRequired]
        [JsonProperty("rule_id")]
        public long RuleId { get; private set; }

        [Required]
        [JsonRequired]
        [JsonProperty("user_id")]
        public long UserId { get; private set; }

        [Required]
        [JsonRequired]
        [JsonProperty("public_key")]
        public string PublicKey { get; private set; }

        [Required]
        [JsonRequired]
        [JsonProperty("hash")]
        public string Hash { get; private set; }

        [JsonProperty("values")]
        public List<double> Values { get; private set; }

        public RuleActionForm(long rule_id, long user_id, string public_key, string hash, List<double> values)
        {
            RuleId = rule_id;
            UserId = user_id;
            PublicKey = public_key;
            Hash = hash;
            Values = values;
        }

        public override bool IsValid => true;
    }
}