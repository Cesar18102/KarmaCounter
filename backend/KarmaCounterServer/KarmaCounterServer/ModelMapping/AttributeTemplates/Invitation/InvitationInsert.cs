namespace KarmaCounterServer.ModelMapping.AttributeTemplates.Invitation
{
    public class InvitationInsert : DbMappingAttribute
    {
        public InvitationInsert(string name, object defaultValue = null) : base(name, true, defaultValue: defaultValue) { }
    }
}