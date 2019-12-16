namespace KarmaCounterServer.ModelMapping.AttributeTemplates.Membership
{
    public class MembershipUpdate : DbMappingAttribute
    {
        public MembershipUpdate(string name) : base(name, true) { }
    }
}