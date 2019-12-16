using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

using KarmaCounterServer.Model;

namespace KarmaCounterServer.Dto
{
    public class RuleForm : DtoForm
    {
        [Required]
        [JsonRequired]
        [JsonProperty("title")]
        public string Title { get; private set; }

        [JsonProperty("text")]
        public string Text { get; private set; }

        [Required]
        [JsonRequired]
        [JsonProperty("fee_formula")]
        public string FeeFormula { get; private set; }

        [Required]
        [JsonRequired]
        [JsonProperty("group_id")]
        public long GroupId { get; private set; }

        [Required]
        [JsonRequired]
        [JsonProperty("creator_session")]
        public Session CreatorSession { get; private set; }

        public RuleForm(string title, string text, string fee_formula, long group_id, Session creator_session)
        {
            Title = title;
            Text = text;
            FeeFormula = fee_formula;
            GroupId = group_id;
            CreatorSession = creator_session;
        }

        public override bool IsValid => true;
    }
}