using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Infrastructure;
using Infrastructure.Data;

namespace Data
{
    internal sealed class UnitOfWorkFactoryProxy : IUnitOfWorkFactoryProxy
    {
        private readonly IDictionary<DatastoreType, IUnitOfWorkFactory> _factories;

        public UnitOfWorkFactoryProxy(params IUnitOfWorkFactory[] factories)
        {
            _factories = factories.ToDictionary(factory => factory.Datastore, factory => factory);

            foreach (var value in (DatastoreType[])Enum.GetValues(typeof(DatastoreType))) {
                if (_factories.ContainsKey(value)) {
                    continue;
                }

                var message = string.Format("UnitOfWorkFactory for DatastoreType '{0}' has not been registered.",
                    value);

                throw new ArgumentException(message);
            }
        }


        [SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope",
            Justification = "UnitOfWork lifecycle is managed by the caller")]
        public IUnitOfWork Create()
        {
            var cache = new UnitOfWorkCache(datastore => GetFactory(datastore).Create());

            return new UnitOfWorkProxy(cache);
        }

#if DEBUG
        void IUnitOfWorkFactoryProxy.Rebuild(DatastoreType datastore)
        {
            GetFactory(datastore).Rebuild();
        }
#endif

        private IUnitOfWorkFactory GetFactory(DatastoreType datastore)
        {
            IUnitOfWorkFactory factory;

            if (!_factories.TryGetValue(datastore, out factory)) {
                throw new NotSupportedException("Specified datastore is not supported.");
            }

            return factory;
        }
    }
}
