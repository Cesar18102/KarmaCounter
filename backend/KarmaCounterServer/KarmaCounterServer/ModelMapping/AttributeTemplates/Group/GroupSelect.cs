namespace KarmaCounterServer.ModelMapping.AttributeTemplates.Group
{
    public class GroupSelect : DbMappingAttribute
    {
        public GroupSelect(string name, string alias = "") : base(name, false, alias) { }
    }
}