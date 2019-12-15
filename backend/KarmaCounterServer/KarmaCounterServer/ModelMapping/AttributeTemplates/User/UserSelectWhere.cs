namespace KarmaCounterServer.ModelMapping.AttributeTemplates.User
{
    public class UserSelectWhere : DbMappingAttribute
    {
        public UserSelectWhere(string name, object defaultValue = null) : base(name, true, defaultValue: defaultValue) { }
    }
}