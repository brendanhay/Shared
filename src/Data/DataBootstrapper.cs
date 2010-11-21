using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Data.Configuration;
using Infrastructure;
using Infrastructure.Data;
using Infrastructure.Extensions;

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
            var factories = config.Datastores.Cast<Datastore>().Select(LoadFactory);
            var proxy = new UnitOfWorkFactoryProxy(factories);

            locator.Inject<IUnitOfWorkFactory>(proxy);
        }

        private static IUnitOfWorkFactory LoadFactory(Datastore datastore)
        {
            var type = Type.GetType(datastore.UnitOfWorkFactory, LoadAssembly,
                (assembly, name, insensitive) => assembly.GetType(name, true, insensitive));

            var factory = LoadFactory(type, datastore);

            return factory;
        }

        private static IUnitOfWorkFactory LoadFactory(Type type, Datastore datastore)
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

            if (instance == null) {
                throw new InvalidCastException(datastore.UnitOfWorkFactory + " is not a valid IUnitOfWorkFactory.");
            }

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
            var path = Path.Combine(assembly.CodeBaseDirectory(), assemblyName + ".dll");

            return Assembly.LoadFile(path);
        }
    }
}
