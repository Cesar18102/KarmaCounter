namespace KarmaCounterServer.ModelMapping.AttributeTemplates.Action
{
    public class ActionInsert : DbMappingAttribute
    {
        public ActionInsert(string name, object defaultValue = null) : base(name, true, defaultValue: defaultValue) { }
    }
}