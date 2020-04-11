using System;
using System.Collections.Generic;

namespace NovaPagedList
{
    /// <summary>
    /// Represents a subset of items (page) of a greater list (superset), usually a database table.
    /// </summary>
    /// <typeparam name="T">The type of the listed items.</typeparam>
    public interface IPagedList<out T> : IReadOnlyList<T>, IPagedListMetadata
    {
    }
}
