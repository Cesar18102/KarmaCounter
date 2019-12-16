namespace KarmaCounterServer.ModelMapping.AttributeTemplates.Rule
{
    public class RuleInsert : DbMappingAttribute
    {
        public RuleInsert(string name, object defaultValue = null) : base(name, true, defaultValue : defaultValue) { }
    }
}