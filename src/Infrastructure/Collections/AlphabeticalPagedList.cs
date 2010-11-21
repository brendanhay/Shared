using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Collections
{
    public interface IAlphabeticalPagedList
    {
        IList<ILetter> Alphabet { get; }

        char? FirstWithRecords { get; }
    }

    public interface IAlphabeticalPagedList<T> : IAlphabeticalPagedList, IList<T> { }

    public interface ILetter
    {
        char Char { get; }

        bool Current { get; }

        bool HasRecords { get; }
    }

    public sealed class AlphabeticalPagedList<T> : List<T>, IAlphabeticalPagedList<T>
    {
        private static readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

        // Make sure the field we're doing the select on (ie. Faq.Title) is indexed in NHibernate!
        public AlphabeticalPagedList(IEnumerable<T> enumerable, Func<T, string> selector, char start)
        {
            Alphabet = new List<ILetter>();

            // Check if the letter found anything, if not check if we have any records at all
            foreach (var @char in _alphabet) {
                var letter = new LetterRecord {
                    Char = @char,
                    HasRecords = enumerable.Any(item => StartsWith(@char, selector)(item)),
                    Current = @char.ToString().Equals(start.ToString(),
                        StringComparison.OrdinalIgnoreCase)
                };

                Alphabet.Add(letter);

                // If we haven't found any records so far, but this letter has some, set it
                if (!FirstWithRecords.HasValue && letter.HasRecords) {
                    FirstWithRecords = letter.Char;
                }
            }

            AddRange(enumerable.Where(StartsWith(start, selector)));
        }

        public IList<ILetter> Alphabet { get; private set; }

        public char? FirstWithRecords { get; private set; }

        private class LetterRecord : ILetter
        {
            public char Char { get; set; }

            public bool Current { get; set; }

            public bool HasRecords { get; set; }
        }

        private static Func<T, bool> StartsWith(char start, Func<T, string> selector)
        {
            return item => {
                var prefix = selector(item);

                return string.IsNullOrEmpty(prefix)
                    ? false
                    : prefix.StartsWith(start.ToString(), StringComparison.OrdinalIgnoreCase);
            };
        }
    }
}
