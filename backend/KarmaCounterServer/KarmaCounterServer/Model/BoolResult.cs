using Newtonsoft.Json;

using KarmaCounterServer.ModelMapping;

namespace KarmaCounterServer.Model
{ 
    [DbMappingTable("", "")]
    public class BoolResult : IModelElement
    {
        [DbMapping("result")]
        [JsonProperty("result")]
        public bool Result { get; private set; }
    }
}