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
        [ActionSelectWhereUser("id")]
        [ActionSelectByGroup("id", "user_id")]
        [ActionSelect("id", "user_id")]
        [ActionInsert("id")]

        [OwnershipSelectWhereUser("id")]
        [OwnershipSelect("id", "user_id")]

        [InvitationSelectWhereUser("id")]
        [InvitationSelect("id", "invitee_id")]
        [InvitationInsert("id")]

        [MembershipSelect("id", "member_id")]
        [MembershipUpdate("id")]
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

        [ActionSelectByGroup("login")]
        [ActionSelect("login")]
        [OwnershipSelect("login")]
        [InvitationSelect("login")]
        [MembershipSelect("login")]
        
        [GroupSelectSecure("login")]
        [GroupSelect("login")]

        [UserSelectWhereLogin("login")]
        [UserUpdate("login")]
        [UserSelect("login")]
        [UserInsert("login")]

        [JsonProperty("login")]
        public string Login { get; private set; }

        [UserInsert("pwd")]
        [JsonIgnore]
        public string Password { get; private set; }

        [ActionSelectByGroup("email")]
        [ActionSelect("email")]
        [OwnershipSelect("email")]
        [InvitationSelect("email")]
        [MembershipSelect("email")]

        [GroupSelectSecure("email")]
        [GroupSelect("email")]

        [UserUpdate("email")]
        [UserSelect("email")]
        [UserInsert("email")]

        [JsonProperty("email")]
        public string Email { get; private set; }

        [ActionSelectByGroup("global_karma")]
        [ActionSelect("global_karma")]
        [OwnershipSelect("global_karma")]
        [InvitationSelect("global_karma")]
        [MembershipSelect("global_karma")]

        [GroupSelectSecure("global_karma")]
        [GroupSelect("global_karma")]

        [UserUpdate("global_karma")]
        [UserSelect("global_karma")]
        [UserInsert("global_karma")]

        [JsonProperty("global_karma")]
        public double GlobalKarma { get; private set; }

        public User() { }

        public User(long id) => Id = id;

        public User(string login) => Login = login;

        public User(string login, string pwd, string email, double global_karma) : this(login)
        {
            Password = pwd;
            Email = email;
            GlobalKarma = global_karma;
        }

        public User(User user, double karma) : this(user.Id, user.Login, user.Password, user.Email, karma) { }

        public User(long id, string login, string pwd, string email, double global_karma) : this(login, pwd, email, global_karma) => Id = id;
    }
}