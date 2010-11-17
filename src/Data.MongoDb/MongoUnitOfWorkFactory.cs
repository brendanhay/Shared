using MongoDB;
using MongoDB.Configuration;
using Data.MongoDb;
using Infrastructure;
using Infrastructure.Data;

namespace Data.MongoDb
{
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

        void IUnitOfWorkFactory.Rebuild()
        {
#if DEBUG
            _mongo.Connect();

            var selector = new Document();

            foreach (var database in _mongo.GetDatabases()) {
                foreach (var collection in database.GetCollectionNames()) {
                    database.GetCollection(collection).Remove(selector);
                }
            }
#endif
        }
    }
}