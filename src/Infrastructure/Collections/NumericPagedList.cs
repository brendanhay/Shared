using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Collections
{
    public interface INumericPagedList
    {
        int PageIndex { get; }

        int PageSize { get; }

        int TotalCount { get; }

        int TotalPages { get; }

        bool HasPreviousPage { get; }

        bool HasNextPage { get; }
    }

    public interface INumericPagedList<T> : INumericPagedList, IList<T> { }

    public sealed class NumericPagedList<T> : List<T>, INumericPagedList<T>
    {
        public NumericPagedList(IQueryable<T> source, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex == 0 ? 1 : pageIndex;
            PageSize = pageSize;
            TotalCount = source.Count();
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);

            AddRange(source.Skip((PageIndex - 1) * PageSize).Take(PageSize));
        }

        public int PageIndex { get; private set; }

        public int PageSize { get; private set; }

        public int TotalCount { get; private set; }

        public int TotalPages { get; private set; }

        public bool HasPreviousPage
        {
            get { return (PageIndex > 1); }
        }

        public bool HasNextPage
        {
            get { return (PageIndex < TotalPages); }
        }
    }
}
