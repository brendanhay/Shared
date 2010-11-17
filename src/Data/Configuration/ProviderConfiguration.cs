﻿using System.Configuration;
using System.IO;
using System.Reflection;

namespace Data.Configuration
{
    internal sealed class ProviderConfiguration : ConfigurationSection
    {
        private const string SECTION_NAME = "providers";

        public static ProviderConfiguration LoadConfigurationFromFile()
        {
            // Get full file path, and check if it exists
            var file = (Assembly.GetExecutingAssembly().CodeBase + ".config")
                .Replace("file:///", "");

            if (!File.Exists(file)) {
                throw new FileNotFoundException(file + " was not found.");
            }

            // set up a exe configuration map - specify the file name of the DLL's config file
            var map = new ExeConfigurationFileMap();
            map.ExeConfigFilename = file;

            // now grab the configuration from the ConfigManager
            var config = ConfigurationManager.OpenMappedExeConfiguration(map,
                ConfigurationUserLevel.None);

            // now grab the section you're interested in from the opened config - 
            // as a sample, I'm grabbing the <appSettings> section
            var section = config.GetSection(SECTION_NAME) as ProviderConfiguration;

            // check for null to be safe, and then get settings from the section
            if (section == null) {
                throw new ConfigurationErrorsException(SECTION_NAME + " section was not found.");
            }

            return section;
        }

        [ConfigurationProperty("datastores", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(DatastoreCollection), AddItemName = "datastore")]
        public DatastoreCollection Datastores
        {
            get { return (DatastoreCollection)this["datastores"]; }
            set { this["datastores"] = value; }
        }
    }
}
