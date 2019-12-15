using System;

namespace KarmaCounterServer.ModelMapping
{
    public class DbMappingForeignAttribute : Attribute
    {
        public string InnerName { get; private set; }
        public string OuterName { get; private set; }

        public DbMappingForeignAttribute(string innerName, string outerName)
        {
            InnerName = innerName;
            OuterName = outerName;
        }
    }
}