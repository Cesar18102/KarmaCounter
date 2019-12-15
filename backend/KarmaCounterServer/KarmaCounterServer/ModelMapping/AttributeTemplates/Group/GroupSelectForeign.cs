namespace KarmaCounterServer.ModelMapping.AttributeTemplates.Group
{
    public class GroupSelectForeign : DbMappingForeignAttribute
    {
        public GroupSelectForeign(string innerName, string outerName) : base(innerName, outerName) { }
    }
}