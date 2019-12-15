namespace KarmaCounterServer.ModelMapping.AttributeTemplates
{
    /// <summary>
    /// Don't map any field with this attribute - just use it in ModelMapper.MapFromModel if no foreign key is needed
    /// </summary>
    public class ForeignIgnore : DbMappingForeignAttribute
    {
        public ForeignIgnore(string innerName, string outerName) : base(innerName, outerName) { }
    }
}