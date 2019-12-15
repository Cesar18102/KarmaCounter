namespace KarmaCounterServer.ModelMapping.AttributeTemplates.Invitation
{
    public class InvitationSelectWhereUser : DbMappingAttribute
    {
        public InvitationSelectWhereUser(string name, object defaultValue = null) : base(name, true, defaultValue : defaultValue) { }
    }
}