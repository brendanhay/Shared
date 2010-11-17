using System.Collections.Generic;
using System.Reflection;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace Data.NHibernate
{
    internal sealed class SessionFactory
    {
        private readonly IDictionary<string, ISessionFactory> _factories
            = new Dictionary<string, ISessionFactory>();

        private readonly object _lock = new object();
        private readonly Assembly _mappings;
        private readonly string _connection;

        public SessionFactory(Assembly mappings, string connection)
        {
            _mappings = mappings;
            _connection = connection;
        }

        public ISessionFactory Create()
        {
            ISessionFactory factory;

            lock (_lock) {
                if (!_factories.TryGetValue(_connection, out factory)) {
                    var configuration = Configure();

                    factory = configuration.BuildSessionFactory();

                    _factories.Add(_connection, factory);
                }
            }

            return factory;
        }

        internal Configuration Configure()
        {
            var listeners = new[] { new AuditEventListener() };

            return Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008.ConnectionString(_connection))
                .Mappings(config => FluentMappings.AddMappings(_mappings, config))
                .ExposeConfiguration(config => config.EventListeners.PreUpdateEventListeners = listeners)
                .ExposeConfiguration(config => config.EventListeners.PreInsertEventListeners = listeners)
                .BuildConfiguration();
        }

        internal void Rebuild()
        {
            var configuration = Configure();
            var schema = new SchemaExport(configuration);

            schema.Drop(false, true);
            schema.Create(false, true);
        }
    }
}
