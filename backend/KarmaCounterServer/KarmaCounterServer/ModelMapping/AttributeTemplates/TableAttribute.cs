namespace KarmaCounterServer.ModelMapping.AttributeTemplates
{
    public class TableAttribute : DbMappingTableAttribute
    {
        public TableAttribute(string tableName, string primaryKey) : base(tableName, primaryKey) { }
    }
}