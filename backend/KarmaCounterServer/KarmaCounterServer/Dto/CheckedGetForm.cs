using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

using KarmaCounterServer.Model;

namespace KarmaCounterServer.Dto
{
    public class CheckedGetForm : DtoForm
    {
        [Required]
        [JsonRequired]
        [JsonProperty("session")]
        public Session Session { get; private set; }

        public CheckedGetForm(Session session) => Session = session;

        public override bool IsValid => true;
    }
}