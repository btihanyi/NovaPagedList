#if !NETSTANDARD2_0
using System;
using System.Collections.Generic;

namespace NovaPagedList
{
    /// <summary>
    /// Represents an asynchronously enumerable subset of items (page) of a greater list (superset), usually a database table.
    /// </summary>
    /// <typeparam name="T">The type of the listed items.</typeparam>
    public interface IAsyncPagedList<out T> : IAsyncEnumerable<T>, IPagedListMetadata
    {
    }
}
#endif
