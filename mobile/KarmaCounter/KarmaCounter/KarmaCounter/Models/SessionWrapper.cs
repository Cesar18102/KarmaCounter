using Newtonsoft.Json;

using KarmaCounter.ModelMapping.ModelMappingAttributes;

namespace KarmaCounter.Models
{
    public class SessionWrapper : IModelElement
    {
        [CreateGroup("creator_session")]

        [JsonProperty("session")]
        public Session CurrentUserSession { get; set; }
    }
}
