using Newtonsoft.Json;

using KarmaCounterServer.ModelMapping.AttributeTemplates;
using KarmaCounterServer.ModelMapping.AttributeTemplates.Group;
using KarmaCounterServer.ModelMapping.AttributeTemplates.Ownership;

namespace KarmaCounterServer.Model
{
    [Table("group_owners", "id")]
    public class Ownership : IModelElement
    {
        [OwnershipSelect("id")]
        [GroupSelect("id", "ownership_id")]

        [JsonProperty("id")]
        public long Id { get; private set; }

        [OwnershipSelectForeign("user_id", "id")]

        [GroupSelectForeign("user_id", "id")]
        [GroupInsert("user_id")]

        [JsonProperty("owner")]
        public User Owner { get; private set; }

        //[JsonProperty("group")]
        //public Group OwnedGroup { get; private set; }

        [OwnershipSelect("public_key")]

        [GroupSelect("public_key")]
        [GroupInsert("public_key")]

        [JsonProperty("public_key")]
        public string PublicKey { get; private set; }

        [OwnershipSelect("private_key")]

        [GroupSelect("private_key")]
        [GroupInsert("private_key")]

        [JsonProperty("private_key")]
        public string PrivateKey { get; private set; }

        public Ownership() { }

        public Ownership(long id) => Id = id;

        public Ownership(User owner) => Owner = owner;

        //public Ownership(Group group) => OwnedGroup = group;

        public Ownership(long id, User owner, string public_key, string private_key) : this(owner, public_key, private_key)
        {
            Id = id;
            //OwnedGroup = owned_group;
        }

        public Ownership(User owner, string public_key, string private_key)
        {
            Owner = owner;
            PublicKey = public_key;
            PrivateKey = private_key;
        }
    }
}