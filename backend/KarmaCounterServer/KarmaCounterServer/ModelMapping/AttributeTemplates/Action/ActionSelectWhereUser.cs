namespace KarmaCounterServer.ModelMapping.AttributeTemplates.Action
{
    public class ActionSelectWhereUser : DbMappingAttribute
    {
        public ActionSelectWhereUser(string name, object defaultValue = null) : base(name, true, defaultValue : defaultValue) { }
    }
}