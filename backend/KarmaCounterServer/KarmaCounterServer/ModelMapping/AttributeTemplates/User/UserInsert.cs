namespace KarmaCounterServer.ModelMapping.AttributeTemplates.User
{
    public class UserInsert : DbMappingAttribute
    {
        public UserInsert(string name, object defaultValue = null) : base(name, true, defaultValue: defaultValue) { }
    }
}