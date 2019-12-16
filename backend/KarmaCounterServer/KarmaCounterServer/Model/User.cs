using System.Collections.Generic;

using Newtonsoft.Json;

using KarmaCounterServer.ModelMapping.AttributeTemplates;
using KarmaCounterServer.ModelMapping.AttributeTemplates.User;
using KarmaCounterServer.ModelMapping.AttributeTemplates.Group;
using KarmaCounterServer.ModelMapping.AttributeTemplates.Membership;
using KarmaCounterServer.ModelMapping.AttributeTemplates.Invitation;
using KarmaCounterServer.ModelMapping.AttributeTemplates.Ownership;
using KarmaCounterServer.ModelMapping.AttributeTemplates.Action;

namespace KarmaCounterServer.Model
{
    [Table("users", "id")]
    public class User : IModelElement
    {
        [ActionSelect("id", "user_id")]
        [ActionInsert("id")]

        [OwnershipSelectWhereUser("id")]
        [OwnershipSelect("id", "user_id")]

        [InvitationSelectWhereUser("id")]
        [InvitationSelect("id", "invitee_id")]
        [InvitationInsert("id")]

        [MembershipSelect("id", "member_id")]
        [MembershipInsert("id")]

        [GroupSelectWhereOwner("id")]
        [GroupSelectSecure("id", "owner_id")]
        [GroupSelect("id", "owner_id")]
        [GroupInsert("id")]
        
        [UserSelectInserted("id")]
        [UserSelectWhere("id")]
        [UserSelect("id")]

        [JsonProperty("id")]
        public long Id { get; private set; }

        [ActionSelect("login")]
        [OwnershipSelect("login")]
        [InvitationSelect("login")]
        [MembershipSelect("login")]
        
        [GroupSelectSecure("login")]
        [GroupSelect("login")]

        [UserSelectWhereLogin("login")]
        [UserSelect("login")]
        [UserInsert("login")]

        [JsonProperty("login")]
        public string Login { get; private set; }

        [UserInsert("pwd")]
        [JsonIgnore]
        public string Password { get; private set; }

        [ActionSelect("email")]
        [OwnershipSelect("email")]
        [InvitationSelect("email")]
        [MembershipSelect("email")]

        [GroupSelectSecure("email")]
        [GroupSelect("email")]

        [UserSelect("email")]
        [UserInsert("email")]

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