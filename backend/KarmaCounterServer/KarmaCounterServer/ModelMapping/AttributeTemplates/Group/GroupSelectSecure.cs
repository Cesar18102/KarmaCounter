namespace KarmaCounterServer.ModelMapping.AttributeTemplates.Group
{
    public class GroupSelectSecure : DbMappingAttribute
    {
        public GroupSelectSecure(string name, string alias = "") : base(name, false, alias) { }
    }
}