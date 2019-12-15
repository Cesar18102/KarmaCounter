namespace KarmaCounterServer.ModelMapping.AttributeTemplates.User
{
    public class UserSelect : DbMappingAttribute
    {
        public UserSelect(string name, string alias = "") : base(name, false, alias) { }
    }
}