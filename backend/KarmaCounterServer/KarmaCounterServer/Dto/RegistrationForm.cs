using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

using KarmaCounterServer.ModelMapping;

namespace KarmaCounterServer.Dto
{
    [JsonObject]
    public class RegistrationForm : DtoForm
    {
        [Required]
        [JsonRequired]
        [JsonProperty("login")]
        public string Login { get; private set; }

        [Required]
        [JsonRequired]
        [JsonProperty("pwd")]
        public string Password { get; private set; }

        [Required]
        [JsonRequired]
        [JsonProperty("email")]
        public string Email { get; private set; }

        public RegistrationForm(string login, string pwd, string email)
        {
            Login = login;
            Password = pwd;
            Email = email;
        }

        public override bool IsValid => true;
    }
}