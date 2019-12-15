using System;

namespace KarmaCounterServer.ModelMapping
{
    public class DbMappingAttribute : Attribute
    {
        public string Name { get; private set; }
        public string Alias { get; private set; }
        public bool MapValue { get; private set; }
        public object DefaultValue { get; private set; }

        public DbMappingAttribute(string name, bool mapValue = false, string alias = "", object defaultValue = null)
        {
            Name = name;
            Alias = alias == "" ? name : alias;
            MapValue = mapValue;
            DefaultValue = defaultValue;
        }
    }
}