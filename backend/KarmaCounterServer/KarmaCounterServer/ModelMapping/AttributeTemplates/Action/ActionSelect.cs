namespace KarmaCounterServer.ModelMapping.AttributeTemplates.Action
{
    public class ActionSelect : DbMappingAttribute
    {
        public ActionSelect(string name, string alias = "") : base(name, false, alias) { }
    }
}