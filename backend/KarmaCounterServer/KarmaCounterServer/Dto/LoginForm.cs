using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

using KarmaCounterServer.ModelMapping;

namespace KarmaCounterServer.Dto
{
    public class LoginForm : DtoForm
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
        [JsonProperty("seed")]
        public string Seed { get; private set; }

        public LoginForm(string login, string pwd, string seed)
        {
            Login = login;
            Password = pwd;
            Seed = seed;
        }

        public override bool IsValid => true;
    }
}