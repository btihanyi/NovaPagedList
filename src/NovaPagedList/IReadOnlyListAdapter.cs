using System;
using System.Collections;
using System.Collections.Generic;

namespace NovaPagedList.Adapters
{
    internal readonly struct IReadOnlyListAdapter<T> : IReadOnlyList<T>
    {
        private readonly IList<T> list;

        public IReadOnlyListAdapter(IList<T> list)
        {
            this.list = list;
        }

        public int Count => list.Count;

        public T this[int index] => list[index];

        public IEnumerator<T> GetEnumerator() => list.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
