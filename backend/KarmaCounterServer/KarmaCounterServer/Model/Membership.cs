﻿using Newtonsoft.Json;

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
        [MembershipInsert("user_id")]

        [JsonProperty("user")]
        public User Member { get; private set; }

        [MembershipSelect("local_karma")]
        [MembershipInsert("local_karma")]

        [JsonProperty("local_karma")]
        public double LocalKarma { get; private set; }

        [MembershipSelect("custom_data")]
        [MembershipInsert("custom_data")]

        [JsonProperty("custom_data")]
        public string CustomData { get; private set; }

        [MembershipSelectForeign("group_id", "id")]
        [MembershipInsert("group_id")]

        [JsonIgnore]
        public Group SourceGroup { get; private set; }

        public Membership() { }

        public Membership(long id) => Id = id;

        public Membership(Group group) => SourceGroup = group;

        public Membership(long id, Group sourceGroup, User member, double initKarma = 0, string customData = "") :
            this(sourceGroup, member, initKarma, customData) => Id = id;

        public Membership(Group sourceGroup, User member, double initKarma = 0, string customData = "")
        {
            SourceGroup = sourceGroup;
            Member = member;
            LocalKarma = initKarma;
            CustomData = customData;
        }
    }
}