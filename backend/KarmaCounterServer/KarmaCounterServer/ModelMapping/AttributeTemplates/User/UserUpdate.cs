namespace KarmaCounterServer.ModelMapping.AttributeTemplates.User
{
    public class UserUpdate : DbMappingAttribute
    {
        public UserUpdate(string name) : base(name, true) { }
    }
}