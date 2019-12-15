using Newtonsoft.Json;

namespace SessionServer.Model
{
    public class BoolResult
    {
        [JsonProperty("result")]
        public bool Result { get; private set; }

        public BoolResult(bool result) => Result = result;
    }
}