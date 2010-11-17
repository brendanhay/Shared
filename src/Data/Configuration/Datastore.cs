using System.Configuration;

namespace Data.Configuration
{
    internal sealed class Datastore : ConfigurationElement
    {
        [ConfigurationProperty("unitOfWorkFactory", IsRequired = true)]
        public string UnitOfWorkFactory
        {
            get { return (string)this["unitOfWorkFactory"]; }
            set { this["unitOfWorkFactory"] = value; }
        }

        [ConfigurationProperty("connection", IsRequired = true)]
        public string Connection
        {
            get { return (string)this["connection"]; }
            set { this["connection"] = value; }
        }

        [ConfigurationProperty("mappings", IsRequired = false, DefaultValue = null)]
        public string Mappings
        {
            get { return (string)this["mappings"]; }
            set { this["mappings"] = value; }
        }
    }

}
