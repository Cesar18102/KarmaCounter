namespace KarmaCounterServer.ModelMapping.AttributeTemplates
{
    public class SelectIgnore : DbMappingAttribute
    {
        public SelectIgnore() : 
            base("ignore") { }
    }
}