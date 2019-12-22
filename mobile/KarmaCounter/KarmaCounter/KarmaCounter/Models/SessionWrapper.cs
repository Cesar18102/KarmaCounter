using Newtonsoft.Json;

using KarmaCounter.ModelMapping.ModelMappingAttributes;

namespace KarmaCounter.Models
{
    public class SessionWrapper : IModelElement
    {
        [AddRule("creator_session")]
        [InviteGroup("inviter_session")]
        [JoinGroup("member_session")]
        [GetOwnership("creator_session")]
        [CreateGroup("creator_session")]

        [JsonProperty("session")]
        public Session CurrentUserSession { get; set; }
    }
}
