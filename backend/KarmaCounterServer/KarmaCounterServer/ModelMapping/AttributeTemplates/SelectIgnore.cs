namespace KarmaCounterServer.ModelMapping.AttributeTemplates
{
    public class SelectIgnore : DbMappingAttribute
    {
        public SelectIgnore(string name, bool mapValue = false, string alias = "", object defaultValue = null) : 
            base(name, mapValue, alias, defaultValue) { }
    }
}