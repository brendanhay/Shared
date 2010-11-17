using System.Configuration;

namespace Data.Configuration
{
    internal sealed class DatastoreConfigurationCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new DatastoreConfiguration();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((DatastoreConfiguration)element).UnitOfWorkFactory;
        }
    }
}
