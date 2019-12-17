using Newtonsoft.Json;

namespace KarmaCounter.Models
{
    public class User : IModelElement
    {
        [JsonProperty("id")]
        public long Id { get; private set; }

        [JsonProperty("login")]
        public string Login { get; private set; }

        [JsonProperty("pwd")]
        public string Password { get; private set; }

        [JsonProperty("email")]
        public string Email { get; private set; }

        [JsonProperty("global_karma")]
        public double Karma { get; private set; }

        [JsonConstructor]
        public User(long id, string login, string pwd, string email, double global_karma)
        {
            Id = id;
            Login = login;
            Password = pwd;
            Email = email;
            Karma = global_karma;
        }
    }
}
