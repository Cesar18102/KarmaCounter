namespace KarmaCounterServer.ModelMapping.AttributeTemplates.Action
{
    public class ActionSelectWhereGroup : DbMappingAttribute
    {
        public ActionSelectWhereGroup(string name, object defaultValue = null) : base(name, true, defaultValue : defaultValue) { }
    }
}