using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Infrastructure;
using Infrastructure.Data;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;

namespace Data.NHibernate
{
    internal sealed class Repository<T> : IRepository<T>
    {
        private readonly ISession _session;
        private readonly INHibernateQueryable<T> _queryable;

        public Repository(ISession session)
        {
            _session = session;
            _queryable = _session.Linq<T>();
        }

        #region IRepository

        public void Add(T item)
        {
            _session.Save(item);
        }

        public void Add(IEnumerable<T> items)
        {
            foreach (var item in items) {
                Add(item);
            }
        }

        public void Update(T item)
        {
            _session.Update(item);
        }

        public void Delete(T item)
        {
            _session.Delete(item);
        }

        public T Get(object id)
        {
            return _session.Get<T>(id);
        }

        public T Find(Expression<Func<T, bool>> where)
        {
            return _queryable.Where(where).FirstOrDefault();
        }

        public IList<T> GetAll()
        {
            return _session.CreateCriteria(typeof(T)).List<T>();
        }

        public IList<T> GetAll(int page, int size, out int total)
        {
            var results = _session.CreateMultiCriteria()
                .Add(_session.CreateCriteria(typeof(T)).SetFirstResult(page * size).SetMaxResults(size))
                .Add(_session.CreateCriteria(typeof(T)).SetProjection(Projections.RowCount()))
                .List();

            total = (int)((IList)results[1])[0];

            return ((IList)results[0]).Cast<T>().ToList();
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

        public System.Linq.Expressions.Expression Expression
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
