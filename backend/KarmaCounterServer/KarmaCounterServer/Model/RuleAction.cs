using System;

using Newtonsoft.Json;

using KarmaCounterServer.ModelMapping.AttributeTemplates;
using KarmaCounterServer.ModelMapping.AttributeTemplates.Action;

namespace KarmaCounterServer.Model
{
    [Table("rules_actions", "id")]
    public class RuleAction : IModelElement
    {
        [ActionSelectInserted("id")]
        [ActionSelectByGroup("id")]
        [ActionSelectWhere("id")]
        [ActionSelect("id")]

        [JsonProperty("id")]
        public long Id { get; private set; }

        [ActionSelectForeign("user_id", "id")]
        [ActionInsert("user_id")]

        [JsonProperty("user")]
        public User ActionSubject { get; private set; }

        [ActionSelectForeign("rule_id", "id")]
        [ActionInsert("rule_id")]

        [JsonProperty("rule")]
        public Rule ActionObject { get; private set; }

        [ActionSelectByGroup("date_time")]
        [ActionSelect("date_time")]
        [ActionInsert("date_time")]

        [JsonProperty("time_stamp")]
        public DateTime TimeStamp { get; private set; }

        [ActionSelectByGroup("violated")]
        [ActionSelect("violated")]
        [ActionInsert("violated")]

        [JsonProperty("violated")]
        public bool Violated { get; private set; }

        [ActionSelectByGroup("fee")]
        [ActionSelect("fee")]
        [ActionInsert("fee")]

        [JsonProperty("fee")]
        public double Fee { get; private set; }

        public RuleAction() { }

        public RuleAction(long id) => Id = id;

        public RuleAction(User user) => ActionSubject = user;

        public RuleAction(Rule rule) => ActionObject = rule;

        public RuleAction(User user, Rule rule, DateTime date_time, bool violated, double fee)
        {
            ActionSubject = user;
            ActionObject = rule;
            TimeStamp = date_time;
            Violated = violated;
            Fee = fee;
        }
    }
}