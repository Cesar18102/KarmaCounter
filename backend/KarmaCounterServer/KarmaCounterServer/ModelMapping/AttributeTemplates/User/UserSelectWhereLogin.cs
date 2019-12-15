namespace KarmaCounterServer.ModelMapping.AttributeTemplates.User
{
    public class UserSelectWhereLogin : DbMappingAttribute
    {
        public UserSelectWhereLogin(string name, object defaultValue = null) : base(name, true, defaultValue: defaultValue) { }
    }
}