namespace KarmaCounterServer.ModelMapping.AttributeTemplates.Group
{
    public class GroupInsert : DbMappingAttribute
    {
        public GroupInsert(string name, object defaultValue = null) : base(name, true, defaultValue:  defaultValue) { }
    }
}