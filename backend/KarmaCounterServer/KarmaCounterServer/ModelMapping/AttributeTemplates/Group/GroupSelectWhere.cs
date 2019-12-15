namespace KarmaCounterServer.ModelMapping.AttributeTemplates.Group
{
    public class GroupSelectWhere : DbMappingAttribute
    {
        public GroupSelectWhere(string name, object defaultValue = null) : base(name, true, defaultValue : defaultValue) { }
    }
}