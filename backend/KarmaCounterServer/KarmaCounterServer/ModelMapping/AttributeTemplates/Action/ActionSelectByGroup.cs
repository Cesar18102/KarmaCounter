namespace KarmaCounterServer.ModelMapping.AttributeTemplates.Action
{
    public class ActionSelectByGroup : DbMappingAttribute
    {
        public ActionSelectByGroup(string name, string alias = "") : base(name, false, alias) { }
    }
}