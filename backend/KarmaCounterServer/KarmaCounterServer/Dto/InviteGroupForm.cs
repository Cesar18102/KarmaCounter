using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

using KarmaCounterServer.Model;

namespace KarmaCounterServer.Dto
{
    public class InviteGroupForm : DtoForm
    {
        [Required]
        [JsonRequired]
        [JsonProperty("group_id")]
        public long GroupId { get; private set; }

        [Required]
        [JsonRequired]
        [JsonProperty("invitee_user_id")]
        public long InviteeId { get; private set; }

        [Required]
        [JsonRequired]
        [JsonProperty("inviter_session")]
        public Session InviterSession { get; private set; }

        public InviteGroupForm(long group_id, long invitee_user_id, Session inviter_session)
        {
            GroupId = group_id;
            InviteeId = invitee_user_id;
            InviterSession = inviter_session;
        }

        public override bool IsValid => true;
    }
}