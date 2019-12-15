namespace KarmaCounterServer.ModelMapping.AttributeTemplates.Ownership
{
    public class OwnershipSelectForeign : DbMappingForeignAttribute
    {
        public OwnershipSelectForeign(string innerName, string outerName) : base(innerName, outerName) { }
    }
}