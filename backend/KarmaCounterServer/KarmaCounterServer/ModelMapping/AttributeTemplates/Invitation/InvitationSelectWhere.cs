namespace KarmaCounterServer.ModelMapping.AttributeTemplates.Invitation
{
    public class InvitationSelectWhere : DbMappingAttribute
    {
        public InvitationSelectWhere(string name, object defaultValue = null) : base(name, true, defaultValue : defaultValue) { }
    }
}