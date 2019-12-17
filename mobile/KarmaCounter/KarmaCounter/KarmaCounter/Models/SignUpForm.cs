using Newtonsoft.Json;

namespace KarmaCounter.Models
{
    public class SignUpForm : IModelElement
    {
        [JsonProperty("login")]
        public string Login { get; private set; }

        [JsonProperty("pwd")]
        public string Password { get; private set; }

        [JsonProperty("email")]
        public string Email { get; private set; }

        public SignUpForm(string login, string pwd, string email)
        {
            Login = login;
            Password = pwd;
            Email = email;
        }
    }
}
