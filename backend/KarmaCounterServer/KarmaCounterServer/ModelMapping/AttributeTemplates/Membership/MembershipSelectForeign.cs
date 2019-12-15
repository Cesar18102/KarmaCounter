namespace KarmaCounterServer.ModelMapping.AttributeTemplates.Membership
{
    public class MembershipSelectForeign : DbMappingForeignAttribute
    {
        public MembershipSelectForeign(string innerName, string outerName) : base(innerName, outerName) { }
    }
}