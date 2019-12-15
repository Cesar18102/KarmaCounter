using System.Collections.Generic;

using Newtonsoft.Json;

using KarmaCounterServer.ModelMapping.AttributeTemplates;
using KarmaCounterServer.ModelMapping.AttributeTemplates.User;
using KarmaCounterServer.ModelMapping.AttributeTemplates.Group;
using KarmaCounterServer.ModelMapping.AttributeTemplates.Membership;

namespace KarmaCounterServer.Model
{
    [Table("users", "id")]
    public class User : IModelElement
    {
        [MembershipInsert("id")]
        [GroupSelect("id", "owner_id")]
        [GroupInsert("id")]
        [UserSelectWhere("id")]
        [UserSelectInserted("id")]
        [UserSelect("id")]
        [JsonProperty("id")]
        public long Id { get; private set; }

        [GroupSelect("login")]
        [UserSelectWhereLogin("login")]
        [UserInsert("login")]
        [UserSelect("login")]
        [JsonProperty("login")]
        public string Login { get; private set; }

        [UserInsert("pwd")]
        [JsonIgnore]
        public string Password { get; private set; }

        [GroupSelect("email")]
        [UserInsert("email")]
        [UserSelect("email")]
        [JsonProperty("email")]
        public string Email { get; private set; }

        public User() { }

        public User(long id) => Id = id;

        public User(string login) => Login = login;

        public User(string login, string pwd, string email) : this(login)
        {
            Password = pwd;
            Email = email;
        }

        public User(long id, string login, string pwd, string email) : this(login, pwd, email) => Id = id;//?
    }
}