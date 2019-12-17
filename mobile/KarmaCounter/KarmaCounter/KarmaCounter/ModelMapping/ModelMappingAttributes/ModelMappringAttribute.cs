using System;

namespace KarmaCounter.ModelMapping.ModelMappingAttributes
{
    public abstract class ModelMappringAttribute : Attribute
    {
        public string Name { get; private set; }
        public ModelMappringAttribute(string name = null) => Name = name;
    }
}
