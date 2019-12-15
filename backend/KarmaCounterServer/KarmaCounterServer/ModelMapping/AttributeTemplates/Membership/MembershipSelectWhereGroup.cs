namespace KarmaCounterServer.ModelMapping.AttributeTemplates.Membership
{
    public class MembershipSelectWhereGroup : DbMappingAttribute
    {
        public MembershipSelectWhereGroup(string name, object defaultValue = null) : base(name, true, defaultValue : defaultValue) { }
    }
}