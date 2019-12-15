using System.Collections.Generic;

using Newtonsoft.Json;

using KarmaCounterServer.ModelMapping.AttributeTemplates;
using KarmaCounterServer.ModelMapping.AttributeTemplates.Group;
using KarmaCounterServer.ModelMapping.AttributeTemplates.Membership;
using KarmaCounterServer.ModelMapping.AttributeTemplates.Invitation;
using KarmaCounterServer.ModelMapping.AttributeTemplates.Ownership;

namespace KarmaCounterServer.Model
{
    [Table("karma_groups", "id")]
    public class Group : IModelElement
    {
        [OwnershipSelectWhereGroup("id")]
        [OwnershipSelect("id", "group_id")]

        [InvitationSelect("id", "group_id")]
        [InvitationInsert("id")]

        [MembershipSelectWhereGroup("id")]
        [MembershipInsert("id")]
        
        [GroupSelectInserted("id")]
        [GroupSelectWhere("id")]
        [GroupSelect("id")]

        [JsonProperty("id")]
        public long Id { get; private set; }

        [OwnershipSelect("name")]

        [InvitationSelect("name")]

        [GroupSelect("name")]
        [GroupInsert("name")]

        [JsonProperty("name")]
        public string Name { get; private set; }

        [OwnershipSelect("description")]

        [InvitationSelect("description")]

        [GroupSelect("description")]
        [GroupInsert("description")]

        [JsonProperty("description")]
        public string Description { get; private set; }

        [OwnershipSelect("is_public")]

        [InvitationSelect("is_public")]

        [GroupSelect("is_public")]
        [GroupInsert("is_public")]

        [JsonProperty("is_public")]
        public bool IsPublic { get; private set; }

        [OwnershipSelect("is_local")]

        [InvitationSelect("is_local")]

        [GroupSelect("is_local")]
        [GroupInsert("is_local")]

        [JsonProperty("is_local")]
        public bool IsLocal { get; private set; }

        [JsonProperty("members")]
        public List<Membership> Members { get; private set; }

        [OwnershipSelectForeign("owner_id", "id")]

        [GroupSelectForeign("owner_id", "id")]
        [GroupInsertForeign("owner_id", "id")]

        [JsonIgnore]
        public Ownership Rights { get; set; }

        [JsonProperty("owner")]
        public User Owner => Rights?.Owner;

        public Group() => Members = new List<Membership>();

        public Group(long id) : this() => Id = id;

        public Group(Ownership ownership) : this() => Rights = ownership;

        public Group(long id, string name, string description,
                     bool is_public, bool is_local, Ownership rights) : 
            this(name, description, is_public, is_local, rights) => Id = id;

        public Group(string name, string description, 
                     bool is_public, bool is_local, Ownership rights) : this()
        {
            Name = name;
            Description = description;
            IsPublic = is_public;
            IsLocal = is_local;
            Rights = rights;
        }
    }
}