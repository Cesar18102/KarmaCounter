using Newtonsoft.Json;

using KarmaCounterServer.ModelMapping.AttributeTemplates;
using KarmaCounterServer.ModelMapping.AttributeTemplates.Rule;
using KarmaCounterServer.ModelMapping.AttributeTemplates.Action;

namespace KarmaCounterServer.Model
{
    [Table("group_rules_fees", "id")]
    public class Rule : IModelElement
    {
        [ActionSelect("id", "rule_id")]
        [ActionInsert("id")]

        [RuleSelectWhere("id")]
        [RuleSelectInserted("id")]
        [RuleSelect("id")]

        [JsonProperty("id")]
        public long Id { get; private set; }

        [ActionSelect("rule_title")]

        [RuleSelect("rule_title")]
        [RuleInsert("rule_title")]

        [JsonProperty("title")]
        public string Title { get; private set; }

        [ActionSelect("rule_text")]

        [RuleSelect("rule_text")]
        [RuleInsert("rule_text")]

        [JsonProperty("text")]
        public string Text { get; private set; }

        [ActionSelect("fee_formula")]

        [RuleSelect("fee_formula")]
        [RuleInsert("fee_formula")]

        [JsonProperty("fee_formula")]
        public string FeeFormula { get; private set; }

        [RuleSelectForeign("group_id", "id")]
        [RuleInsert("group_id")]

        [JsonIgnore]
        public Group SourceGroup { get; private set; }

        public Rule() { }

        public Rule(long id) => Id = id;

        public Rule(Group group) => SourceGroup = group;

        public Rule(string title, string text, string feeFormula, Group group)
        {
            Title = title;
            Text = text;
            FeeFormula = feeFormula;
            SourceGroup = group;
        }
    }
}