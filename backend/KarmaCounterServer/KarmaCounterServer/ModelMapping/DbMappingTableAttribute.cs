using System;

namespace KarmaCounterServer.ModelMapping
{
    public class DbMappingTableAttribute : Attribute
    {
        public string TableName { get; private set; }
        public string PrimaryKey { get; private set; }

        public DbMappingTableAttribute(string tableName, string primaryKey)
        {
            TableName = tableName;
            PrimaryKey = primaryKey;
        }
    }
}