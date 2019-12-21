using System.Collections.Generic;

using Newtonsoft.Json;

using KarmaCounter.ModelMapping.ModelMappingAttributes;

namespace KarmaCounter.Models
{
    public class Group : IModelElement
    {
        [InviteGroup("group_id")]
        [JoinGroup("group_id")]
        [GetGroupById("id")]
        [GetOwnership("group_id")]
        [JsonProperty("id")]
        public long Id { get; private set; }

        [CreateGroup("name")]
        [JsonProperty("name")]
        public string Name { get; private set; }

        [CreateGroup("description")]
        [JsonProperty("description")]
        public string Description { get; private set; }

        [CreateGroup("is_public")]
        [JsonProperty("is_public")]
        public bool IsPublic { get; private set; }

        [CreateGroup("is_local")]
        [JsonProperty("is_local")]
        public bool IsLocal { get; private set; }

        [JsonProperty("members")]
        public List<Membership> Members { get; private set; }

        [JsonProperty("rules")]
        public List<Rule> Rules { get; private set; }

        [JsonProperty("rights")]
        public Ownership Rights { get; private set; }

        [JsonProperty("owner")]
        public User Owner { get; private set; }

        public Group(string name, string description, bool is_public, bool is_local)
        {
            Name = name;
            Description = description;
            IsPublic = is_public;
            IsLocal = is_local;
        }

        [JsonConstructor]
        public Group(long id, string name, string description, bool is_public, bool is_local,
                     List<Membership> members, List<Rule> rules, Ownership rights, User owner) :
            this(name, description, is_public, is_local)
        {
            Id = id;
            Members = members;
            Rules = rules;
            Rights = rights;
            Owner = owner;
        }
    }
}
