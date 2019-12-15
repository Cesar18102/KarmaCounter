using Newtonsoft.Json;

using KarmaCounterServer.ModelMapping.AttributeTemplates;
using KarmaCounterServer.ModelMapping.AttributeTemplates.Invitation;

namespace KarmaCounterServer.Model
{
    [Table("group_invitations", "id")]
    public class Invitation : IModelElement
    {
        [InvitationSelectInserted("id")]
        [InvitationSelectWhere("id")]
        [InvitationSelect("id")]

        [JsonProperty("id")]
        public long Id { get; private set; }

        [InvitationSelectForeign("user_id", "id")]
        [InvitationInsert("user_id")]

        [JsonProperty("invitee")]
        public User Invitee { get; private set; }

        [InvitationSelectForeign("group_id", "id")]
        [InvitationInsert("group_id")]

        [JsonProperty("source_group")]
        public Group SourceGroup { get; private set; }

        public Invitation() { }

        public Invitation(long id) => Id = id;

        public Invitation(User invitee) => Invitee = invitee;

        public Invitation(User invitee, Group source_group)
        {
            Invitee = invitee;
            SourceGroup = source_group;
        }
    }
}