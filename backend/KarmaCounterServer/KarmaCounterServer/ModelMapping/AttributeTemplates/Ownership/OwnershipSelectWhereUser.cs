namespace KarmaCounterServer.ModelMapping.AttributeTemplates.Ownership
{
    public class OwnershipSelectWhereUser : DbMappingAttribute
    {
        public OwnershipSelectWhereUser(string name, object defaultValue = null) : base(name, true, defaultValue : defaultValue) { }
    }
}