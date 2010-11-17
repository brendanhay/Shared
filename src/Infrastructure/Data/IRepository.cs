using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Infrastructure.Data
{
    public interface IRepository<T> : IQueryable<T>
    {
        void Add(T item);

        void Add(IEnumerable<T> items);

        void Update(T item);

        void Delete(T item);

        T Get(object id);

        T Find(Expression<Func<T, bool>> where);

        IList<T> GetAll();

        IList<T> GetAll(int page, int size, out int total);
    }
}
