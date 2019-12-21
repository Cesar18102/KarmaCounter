using Newtonsoft.Json;

namespace KarmaCounter.Models
{
    public class Membership : IModelElement
    {
        [JsonProperty("id")]
        public long Id { get; private set; }

        [JsonProperty("user")]
        public User Member { get; private set; }

        [JsonProperty("local_karma")]
        public double Karma { get; private set; }

        [JsonProperty("custom_data")]
        public string CustomData { get; private set; }

        public Membership(long id, User user, double local_karma, string custom_data)
        {
            Id = id;
            Member = user;
            Karma = local_karma;
            CustomData = custom_data;
        }
    }
}
