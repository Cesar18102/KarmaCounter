namespace KarmaCounterServer.ModelMapping.AttributeTemplates.Membership
{
    public class MembershipSelectWhere : DbMappingAttribute
    {
        public MembershipSelectWhere(string name, object defaultValue = null) : base(name, true, defaultValue : defaultValue) { }
    }
}