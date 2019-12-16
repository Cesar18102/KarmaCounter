using Newtonsoft.Json;

using KarmaCounterServer.ModelMapping.AttributeTemplates;
using KarmaCounterServer.ModelMapping.AttributeTemplates.Membership;

namespace KarmaCounterServer.Model
{
    [Table("group_members", "id")]
    public class Membership : IModelElement
    {
        [MembershipSelectInserted("id")]
        [MembershipSelectWhere("id")]
        [MembershipSelect("id")]

        [JsonProperty("id")]
        public long Id { get; private set; }

        [MembershipSelectForeign("user_id", "id")]
        [MembershipUpdate("user_id")]
        [MembershipInsert("user_id")]

        [JsonProperty("user")]
        public User Member { get; private set; }

        [MembershipUpdate("local_karma")]
        [MembershipSelect("local_karma")]
        [MembershipInsert("local_karma")]

        [JsonProperty("local_karma")]
        public double LocalKarma { get; private set; }

        [MembershipUpdate("custom_data")]
        [MembershipSelect("custom_data")]
        [MembershipInsert("custom_data")]

        [JsonProperty("custom_data")]
        public string CustomData { get; private set; }

        [MembershipSelectForeign("group_id", "id")]
        [MembershipUpdate("group_id")]
        [MembershipInsert("group_id")]

        [JsonIgnore]
        public Group SourceGroup { get; private set; }

        public Membership() { }
        public Membership(long id) => Id = id;
        public Membership(Group group) => SourceGroup = group;
        public Membership(Membership source, Group group) : this(source) => SourceGroup = group;

        public Membership(long id, Group sourceGroup, User member, double initKarma = 0, string customData = "") :
            this(sourceGroup, member, initKarma, customData) => Id = id;

        public Membership(Membership source, double karma) : this(source) => LocalKarma = karma;
        public Membership(Membership source, string custom_data) : this(source) => CustomData = custom_data;
        public Membership(Membership source) : this(source.Id, source.SourceGroup, source.Member, source.LocalKarma, source.CustomData) { }

        public Membership(Group sourceGroup, User member, double initKarma = 0, string customData = "")
        {
            SourceGroup = sourceGroup;
            Member = member;
            LocalKarma = initKarma;
            CustomData = customData;
        }
    }
}