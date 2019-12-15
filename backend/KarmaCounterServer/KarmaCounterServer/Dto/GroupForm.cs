using Newtonsoft.Json;

using KarmaCounterServer.Model;

namespace KarmaCounterServer.Dto
{
    public class GroupForm : DtoForm
    {
        [JsonProperty("name")]
        public string Name { get; private set; }

        [JsonProperty("description")]
        public string Description { get; private set; }

        [JsonProperty("is_public")]
        public bool IsPublic { get; private set; }

        [JsonProperty("is_local")]
        public bool IsLocal { get; private set; }

        [JsonProperty("creator_session")]
        public Session CreatorSession { get; private set; }

        public GroupForm(string name, string description, bool is_public, bool is_local, Session creator_session)
        {
            Name = name;
            Description = description;
            IsPublic = is_public;
            IsLocal = is_local;
            CreatorSession = creator_session;
        }

        public override bool IsValid => true;
    }
}