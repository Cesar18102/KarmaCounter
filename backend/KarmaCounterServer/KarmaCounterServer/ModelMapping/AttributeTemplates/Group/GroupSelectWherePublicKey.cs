namespace KarmaCounterServer.ModelMapping.AttributeTemplates.Group
{
    public class GroupSelectWherePublicKey : DbMappingAttribute
    {
        public GroupSelectWherePublicKey(string name, object defaultValue = null) : base(name, true, defaultValue : defaultValue) { }
    }
}