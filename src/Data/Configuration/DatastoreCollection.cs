using System.Configuration;

namespace Data.Configuration
{
    internal sealed class DatastoreCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new Datastore();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((Datastore)element).UnitOfWorkFactory;
        }
    }
}
