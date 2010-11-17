using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Data.Configuration;
using Infrastructure;
using Infrastructure.Data;

namespace Data
{
    public static class DataBootstrapper
    {
        public static void Setup(IServiceLocator locator)
        {
            Setup(locator, ProviderConfiguration.LoadConfigurationFromFile());
        }

        internal static void Setup(IServiceLocator locator, ProviderConfiguration config)
        {
            foreach (DatastoreConfiguration datastore in config.Datastores) {
                RegisterFactory(locator, datastore);
            }

            locator.Add<IUnitOfWorkFactoryProxy, UnitOfWorkFactoryProxy>(false);
        }

        private static void RegisterFactory(IServiceLocator locator,
            DatastoreConfiguration datastore)
        {
            var type = Type.GetType(datastore.UnitOfWorkFactory, LoadAssembly,
                (assembly, name, insensitive) => assembly.GetType(name, true, insensitive));

            var factory = LoadFactory(type, datastore);

            if (factory != null) {
                locator.Inject(datastore.UnitOfWorkFactory, factory);
            }
        }

        private static IUnitOfWorkFactory LoadFactory(Type type, DatastoreConfiguration datastore)
        {
            var mappings = LoadMappings(datastore.Mappings);
            var ctor = type.GetConstructors().FirstOrDefault();

            if (ctor == null) {
                throw new TypeLoadException("Could not find a constructor for the specified UnitOfWorkFactory.");
            }

            var parameters = mappings == null
                ? new object[] { datastore.Connection }
                : new object[] { datastore.Connection, mappings };

            var instance = ctor.Invoke(parameters) as IUnitOfWorkFactory;

            return instance;
        }

        private static Assembly LoadMappings(string mappings)
        {
            var assembly = default(Assembly);

            if (!string.IsNullOrEmpty(mappings)) {
                assembly = LoadAssembly(mappings);
            }

            return assembly;
        }

        private static Assembly LoadAssembly(AssemblyName assemblyName)
        {
            return LoadAssembly(assemblyName.Name);
        }

        private static Assembly LoadAssembly(string assemblyName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var index = assembly.CodeBase.LastIndexOf('/');
            var location = assembly.CodeBase.Remove(index).Replace("file:///", "");
            var path = Path.Combine(location, assemblyName + ".dll");

            return Assembly.LoadFile(path);
        }
    }
}
