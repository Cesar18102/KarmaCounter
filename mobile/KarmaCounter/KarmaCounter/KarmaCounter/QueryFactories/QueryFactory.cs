using KarmaCounter.ModelMapping;

namespace KarmaCounter.QueryFactories
{
    public abstract class QueryFactory
    {
        protected ModelMapper Mapper { get; set; }

        public QueryFactory() => Mapper = new ModelMapper();
    }
}
