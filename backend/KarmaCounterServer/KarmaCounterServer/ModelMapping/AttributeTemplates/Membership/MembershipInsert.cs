namespace KarmaCounterServer.ModelMapping.AttributeTemplates.Membership
{
    public class MembershipInsert : DbMappingAttribute
    {
        public MembershipInsert(string name, object defaultValue = null) : base(name, true, defaultValue : defaultValue) { }
    }
}