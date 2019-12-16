namespace KarmaCounterServer.ModelMapping.AttributeTemplates.Rule
{
    public class RuleSelectWhereGroup : DbMappingAttribute
    {
        public RuleSelectWhereGroup(string name, object defaultValue = null) : base(name, true, defaultValue : defaultValue) { }
    }
}