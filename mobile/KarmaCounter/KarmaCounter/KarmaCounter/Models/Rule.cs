using Newtonsoft.Json;

using KarmaCounter.ModelMapping.ModelMappingAttributes;

namespace KarmaCounter.Models
{
    public class Rule : IModelElement
    {
        [JsonProperty("id")]
        public long Id { get; private set; }

        [AddRule("title")]
        [JsonProperty("title")]
        public string Title { get; private set; }

        [AddRule("text")]
        [JsonProperty("text")]
        public string Text { get; private set; }

        [AddRule("fee_formula")]
        [JsonProperty("fee_formula")]
        public string FeeFormula { get; private set; }

        [JsonConstructor]
        public Rule(long id, string title, string text, string fee_formula) :
            this(title, text, fee_formula) => Id = id;

        public Rule(string title, string text, string fee_formula)
        {
            Title = title;
            Text = text;
            FeeFormula = fee_formula;
        }
    }
}