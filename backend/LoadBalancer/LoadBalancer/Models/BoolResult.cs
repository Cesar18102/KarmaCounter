using Newtonsoft.Json;

namespace LoadBalancer.Models
{
    [JsonObject]
    public class BoolResult
    {
        [JsonProperty("result")]
        public bool Result { get; private set; }

        public BoolResult(bool result) => Result = result;

        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}