using Newtonsoft.Json;

namespace LoadBalancer.Models
{
    [JsonObject]
    public class BoolResult
    {
        [JsonProperty("Result")]
        public bool Result { get; private set; }

        public BoolResult(bool result) => Result = result;
    }
}