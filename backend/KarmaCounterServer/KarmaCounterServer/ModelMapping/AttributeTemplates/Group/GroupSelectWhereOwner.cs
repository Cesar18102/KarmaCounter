namespace KarmaCounterServer.ModelMapping.AttributeTemplates.Group
{
    public class GroupSelectWhereOwner : DbMappingAttribute
    {
        public GroupSelectWhereOwner(string name, object defaultValue = null) : base(name, true, defaultValue: defaultValue) { }
    }
}