using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

using KarmaCounterServer.Model;

namespace KarmaCounterServer.Dto
{
    public class GetOwnershipForm : DtoForm
    {
        [Required]
        [JsonRequired]
        [JsonProperty("creator_session")]
        public Session CreatorSession { get; private set; }

        [Required]
        [JsonRequired]
        [JsonProperty("group_id")]
        public long GroupId { get; private set; }

        public GetOwnershipForm(Session creator_session, long group_id)
        {
            GroupId = group_id;
            CreatorSession = creator_session;
        }

        public override bool IsValid => true;
    }
}