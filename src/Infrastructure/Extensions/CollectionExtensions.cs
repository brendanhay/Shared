﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace Infrastructure.Extensions
{
    public static class CollectionExtensions
    {
        public static void AddRange<T>(this IList<T> self, IEnumerable<T> enumerable)
        {
            foreach (var item in enumerable) {
                self.Add(item);
            }
        }

        public static bool IsNullOrEmpty<T>(this ICollection<T> self)
        {
            return self == null || self.Count < 1;
        }


        public static IEnumerable<IEnumerable<T>> Partition<T>(this IEnumerable<T> enumerable, int partitionSize)
        {
            /*
            return enumerable
                .Select((item, index) => new { Item = item, Index = index, })
                .GroupBy(item => item.Index / partitionSize)
                .Select(group => group.Select(item => item.Item)                )
                ;
            */

            return new PartitioningEnumerable<T>(enumerable, partitionSize);
        }

        private class PartitioningEnumerable<T> : IEnumerable<IEnumerable<T>>
        {
            private readonly IEnumerable<T> _enumerable;
            private readonly int _partitionSize;

            public PartitioningEnumerable(IEnumerable<T> enumerable, int partitionSize)
            {
                _enumerable = enumerable;
                _partitionSize = partitionSize;
            }

            public IEnumerator<IEnumerable<T>> GetEnumerator()
            {
                return new PartitioningEnumerator<T>(_enumerable.GetEnumerator(), _partitionSize);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }


        private class PartitioningEnumerator<T> : IEnumerator<IEnumerable<T>>
        {
            private readonly IEnumerator<T> _enumerator;
            private readonly int _partitionSize;

            public PartitioningEnumerator(IEnumerator<T> enumerator, int partitionSize)
            {
                _enumerator = enumerator;
                _partitionSize = partitionSize;
            }

            public void Dispose()
            {
                _enumerator.Dispose();
            }

            IEnumerable<T> _current;

            public IEnumerable<T> Current
            {
                get { return _current; }
            }

            object IEnumerator.Current
            {
                get { return _current; }
            }

            public void Reset()
            {
                _current = null;
                _enumerator.Reset();
            }

            public bool MoveNext()
            {
                bool result;

                if (_enumerator.MoveNext()) {
                    _current = new PartitionEnumerable<T>(_enumerator, _partitionSize);
                    result = true;
                } else {
                    _current = null;
                    result = false;
                }

                return result;
            }

        }

        private class PartitionEnumerable<T> : IEnumerable<T>
        {
            private readonly IEnumerator<T> _enumerator;
            private readonly int _partitionSize;

            public PartitionEnumerable(IEnumerator<T> enumerator, int partitionSize)
            {
                _enumerator = enumerator;
                _partitionSize = partitionSize;
            }

            public IEnumerator<T> GetEnumerator()
            {
                return new PartitionEnumerator<T>(_enumerator, _partitionSize);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }


        private class PartitionEnumerator<T> : IEnumerator<T>
        {
            private readonly IEnumerator<T> _enumerator;
            private readonly int _partitionSize;

            private int _count;

            public PartitionEnumerator(IEnumerator<T> enumerator, int partitionSize)
            {
                _enumerator = enumerator;
                _partitionSize = partitionSize;
            }

            public void Dispose()
            {
            }

            public T Current
            {
                get { return _enumerator.Current; }
            }
            object IEnumerator.Current
            {
                get { return _enumerator.Current; }
            }
            public void Reset()
            {
                if (_count > 0) throw new InvalidOperationException();
            }

            public bool MoveNext()
            {
                bool result;

                if (_count < _partitionSize) {
                    if (_count > 0) {
                        result = _enumerator.MoveNext();
                    } else {
                        result = true;
                    }
                    _count++;
                } else {
                    result = false;
                }

                return result;
            }

        }
    }
}
