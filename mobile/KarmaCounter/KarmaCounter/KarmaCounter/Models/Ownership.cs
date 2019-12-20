using Newtonsoft.Json;

namespace KarmaCounter.Models
{
    public class Ownership : IModelElement
    {
        [JsonProperty("id")]
        public long Id { get; private set; }

        [JsonProperty("owner")]
        public User Owner { get; private set; }

        [JsonProperty("public_key")]
        public string PublicKey { get; private set; }

        [JsonProperty("private_key")]
        public string PrivateKey { get; private set; }

        public Ownership(long id, User owner, string public_key, string private_key)
        {
            Id = id;
            Owner = owner;
            PublicKey = public_key;
            PrivateKey = private_key;
        }
    }
}