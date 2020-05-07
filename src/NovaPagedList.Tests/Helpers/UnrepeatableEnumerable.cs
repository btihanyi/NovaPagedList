using System;
using System.Collections;
using System.Collections.Generic;

namespace NovaPagedList.Tests
{
    public class UnrepeatableEnumerable<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> enumerable;
        private bool enumerated = false;

        public UnrepeatableEnumerable(IEnumerable<T> enumerable)
        {
            this.enumerable = enumerable;
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (enumerated)
            {
                throw new InvalidOperationException("The collection has already been enumerated.");
            }

            enumerated = true;
            return enumerable.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public static class UnrepeatableEnumerableExtensions
    {
        public static UnrepeatableEnumerable<T> Unrepeatable<T>(this IEnumerable<T> enumerable)
        {
            return new UnrepeatableEnumerable<T>(enumerable);
        }
    }
}
