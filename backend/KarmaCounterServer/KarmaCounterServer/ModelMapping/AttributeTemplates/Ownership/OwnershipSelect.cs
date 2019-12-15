namespace KarmaCounterServer.ModelMapping.AttributeTemplates.Ownership
{
    public class OwnershipSelect : DbMappingAttribute
    {
        public OwnershipSelect(string name, string alias = "") : base(name, false, alias) { }
    }
}