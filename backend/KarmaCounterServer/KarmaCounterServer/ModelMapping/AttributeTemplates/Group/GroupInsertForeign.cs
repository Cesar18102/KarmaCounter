namespace KarmaCounterServer.ModelMapping.AttributeTemplates.Group
{
    public class GroupInsertForeign : DbMappingForeignAttribute
    {
        public GroupInsertForeign(string innerName, string outerName) : base(innerName, outerName) { }
    }
}