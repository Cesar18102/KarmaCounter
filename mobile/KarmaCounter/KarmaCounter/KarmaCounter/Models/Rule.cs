using Newtonsoft.Json;

namespace KarmaCounter.Models
{
    public class Rule : IModelElement
    {
        [JsonProperty("id")]
        public long Id { get; private set; }

        [JsonProperty("title")]
        public string Title { get; private set; }

        [JsonProperty("text")]
        public string Text { get; private set; }

        [JsonProperty("fee_formula")]
        public string FeeFormula { get; private set; }

        public Rule(long id, string title, string text, string fee_formula)
        {
            Id = id;
            Title = title;
            Text = text;
            FeeFormula = fee_formula;
        }
    }
}