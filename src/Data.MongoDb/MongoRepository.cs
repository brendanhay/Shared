using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Infrastructure;
using Infrastructure.Data;
using MongoDB;

namespace Data.MongoDb
{
    internal sealed class MongoRepository<T> : IRepository<T> where T : class
    {
        private const string ID_COLUMN = "_id";

        private readonly IMongoCollection<T> _collection;
        private readonly IQueryable<T> _queryable;

        public MongoRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<T>(typeof(T).Name);
            _queryable = _collection.Linq();
        }

        #region IRepository<T>

        public void Add(T item)
        {
            //SetAuditable(item, auditable => auditable.UpdatedAt = DateTime.Now);
            //SetAuditable(item, auditable => auditable.CreatedAt = DateTime.Now);

            _collection.Insert(item);
        }

        public void Add(IEnumerable<T> items)
        {
            _collection.Insert(items);
        }

        public void Update(T item)
        {
            //SetAuditable(item, auditable => auditable.UpdatedAt = DateTime.Now);

            _collection.Save(item);
        }

        public void Delete(T item)
        {
            _collection.Remove(item);
        }

        public T Get(object id)
        {
            return _collection.FindOne(new Document { { ID_COLUMN, id } });
        }

        public T Find(Expression<Func<T, bool>> where)
        {
            return _collection.FindOne(where);
        }

        public IList<T> GetAll()
        {
            return _collection.FindAll().Documents.ToList();
        }

        public IList<T> GetAll(int page, int size, out int total)
        {
            // TODO: perform count
            total = 0;

            return _collection.FindAll().Skip(page * size).Documents.Take(size).ToList();
        }

        #endregion

        #region IQueryable<T>

        public IEnumerator<T> GetEnumerator()
        {
            return _queryable.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Type ElementType
        {
            get { return _queryable.ElementType; }
        }

        public Expression Expression
        {
            get { return _queryable.Expression; }
        }

        public IQueryProvider Provider
        {
            get { return _queryable.Provider; }
        }

        #endregion
    }
}
