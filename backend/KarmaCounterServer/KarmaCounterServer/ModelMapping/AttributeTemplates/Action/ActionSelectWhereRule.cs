namespace KarmaCounterServer.ModelMapping.AttributeTemplates.Action
{
    public class ActionSelectWhereRule : DbMappingAttribute
    {
        public ActionSelectWhereRule(string name, object defaultValue = null) : base(name, true, defaultValue : defaultValue) { }
    }
}