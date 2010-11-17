using System.Diagnostics.CodeAnalysis;
using Infrastructure;
using Infrastructure.Data;
using MongoDB;
using MongoDB.Configuration;

namespace Data.MongoDb
{
    [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable",
        Justification = "Type is not visible outside the assembly and UnitOfWork lifecycle is managed by the caller")]
    internal sealed class MongoUnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly Mongo _mongo;

        public MongoUnitOfWorkFactory(string connection)
        {
            var builder = new MongoConfigurationBuilder();

            builder.ConnectionString(connection);
            builder.Mapping(m => m.DefaultProfile(profile => profile
                .UseIdUnsavedValueConvention(new UnsavedIdConvention())));

            _mongo = new Mongo(builder.BuildConfiguration());
        }

        public DatastoreType Datastore { get { return DatastoreType.Document; } }

        public IUnitOfWork Create()
        {
            return new MongoUnitOfWork(_mongo);
        }

#if DEBUG
        void IUnitOfWorkFactory.Rebuild()
        {
            _mongo.Connect();

            var selector = new Document();

            foreach (var database in _mongo.GetDatabases()) {
                foreach (var collection in database.GetCollectionNames()) {
                    database.GetCollection(collection).Remove(selector);
                }
            }
        }
#endif
    }
}