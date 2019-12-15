using Newtonsoft.Json;

using KarmaCounterServer.Model;

namespace KarmaCounterServer.Dto
{
    public class JoinGroupForm : DtoForm
    {
        public override bool IsValid => true;

        [JsonProperty("group_id")]
        public long GroupId { get; private set; }

        [JsonProperty("member_session")]
        public Session MemberSession { get; private set; }

        public JoinGroupForm(long group_id, Session member_session)
        {
            GroupId = group_id;
            MemberSession = member_session;
        }
    }
}