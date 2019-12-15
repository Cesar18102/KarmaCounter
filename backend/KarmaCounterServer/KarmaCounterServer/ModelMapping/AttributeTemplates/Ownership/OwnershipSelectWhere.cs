namespace KarmaCounterServer.ModelMapping.AttributeTemplates.Ownership
{
    public class OwnershipSelectWhere : DbMappingAttribute
    {
        public OwnershipSelectWhere(string name, object defaultValue = null) : base(name, true, defaultValue : defaultValue) { }
    }
}