namespace KarmaCounterServer.ModelMapping.AttributeTemplates.Rule
{
    public class RuleSelect : DbMappingAttribute
    {
        public RuleSelect(string name, string alias = "") : base(name, false, alias) { }
    }
}