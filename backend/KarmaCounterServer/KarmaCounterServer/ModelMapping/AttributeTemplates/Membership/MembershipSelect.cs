namespace KarmaCounterServer.ModelMapping.AttributeTemplates.Membership
{
    public class MembershipSelect : DbMappingAttribute
    {
        public MembershipSelect(string name, string alias = "") : base(name, false, alias) { }
    }
}