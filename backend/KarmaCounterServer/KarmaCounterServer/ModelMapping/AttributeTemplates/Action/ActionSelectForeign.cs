namespace KarmaCounterServer.ModelMapping.AttributeTemplates.Action
{
    public class ActionSelectForeign : DbMappingForeignAttribute
    {
        public ActionSelectForeign(string innerName, string outerName) : base(innerName, outerName) { }
    }
}