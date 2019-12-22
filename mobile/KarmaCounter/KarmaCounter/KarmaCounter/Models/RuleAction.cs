using System;

using Newtonsoft.Json;

namespace KarmaCounter.Models
{
    public class RuleAction : IModelElement
    {
        [JsonProperty("id")]
        public long Id { get; private set; }

        [JsonProperty("user")]
        public User ActionSubject { get; private set; }

        [JsonProperty("rule")]
        public Rule ActionObject { get; private set; }

        [JsonProperty("time_stamp")]
        public DateTime TimeStamp { get; private set; }

        [JsonProperty("violated")]
        public bool Violated { get; private set; }

        [JsonProperty("fee")]
        public double Fee { get; private set; }

        public RuleAction(long id, User user, Rule rule, DateTime time_stamp, bool violated, double fee)
        {
            Id = id;
            ActionSubject = user;
            ActionObject = rule;
            TimeStamp = time_stamp;
            Violated = violated;
            Fee = fee;
        }
    }
}
