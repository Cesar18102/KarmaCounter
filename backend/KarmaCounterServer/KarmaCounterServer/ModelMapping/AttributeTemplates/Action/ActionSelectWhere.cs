namespace KarmaCounterServer.ModelMapping.AttributeTemplates.Action
{
    public class ActionSelectWhere : DbMappingAttribute
    {
        public ActionSelectWhere(string name, object defaultValue = null) : base(name, true, defaultValue : defaultValue) { }
    }
}