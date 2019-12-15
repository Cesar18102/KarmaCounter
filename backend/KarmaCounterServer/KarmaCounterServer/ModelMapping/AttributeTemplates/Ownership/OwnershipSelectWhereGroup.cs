namespace KarmaCounterServer.ModelMapping.AttributeTemplates.Ownership
{
    public class OwnershipSelectWhereGroup : DbMappingAttribute
    {
        public OwnershipSelectWhereGroup(string name, object defaultValue = null) : base(name, true, defaultValue : defaultValue) { }
    }
}