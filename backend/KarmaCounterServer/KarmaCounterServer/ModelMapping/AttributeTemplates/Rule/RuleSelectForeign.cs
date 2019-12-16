namespace KarmaCounterServer.ModelMapping.AttributeTemplates.Rule
{
    public class RuleSelectForeign : DbMappingForeignAttribute
    {
        public RuleSelectForeign(string innerName, string outerName) : base(innerName, outerName) { }
    }
}