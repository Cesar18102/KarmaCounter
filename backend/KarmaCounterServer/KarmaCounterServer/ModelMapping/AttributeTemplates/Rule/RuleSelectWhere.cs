namespace KarmaCounterServer.ModelMapping.AttributeTemplates.Rule
{
    public class RuleSelectWhere : DbMappingAttribute
    {
        public RuleSelectWhere(string name, object defaultValue = null) : base(name, true, defaultValue : defaultValue) { }
    }
}