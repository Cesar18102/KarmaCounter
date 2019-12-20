using Newtonsoft.Json;

using KarmaCounterServer.Dto;

namespace KarmaCounterServer.Model
{
    public class PaymentInfo : IModelElement
    {
        public GroupForm Form { get; private set; }

        [JsonProperty("info")]
        public string Info { get; private set; }

        public PaymentInfo(string info) => Form = JsonConvert.DeserializeObject<GroupForm>(info);
    }
}