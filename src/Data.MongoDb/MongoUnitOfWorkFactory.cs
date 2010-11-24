using Infrastructure.Data;
using MongoDB;

namespace Data.MongoDb
{
    internal sealed class MongoUnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly string _connection;

        public MongoUnitOfWorkFactory(string connection)
        {
            _connection = connection;
        }

        public DatastoreType Datastore { get { return DatastoreType.Document; } }

        public IUnitOfWork Create()
        {
            return new MongoUnitOfWork(_connection);
        }

#if DEBUG
        void IUnitOfWorkFactory.Rebuild()
        {
            using (var mongo = new Mongo(_connection)) {
                mongo.Connect();

                var selector = new Document();

                foreach (var database in mongo.GetDatabases()) {
                    foreach (var collection in database.GetCollectionNames()) {
                        database.GetCollection(collection).Remove(selector);
                    }
                }

                mongo.Disconnect();
            }
        }
#endif
    }
}