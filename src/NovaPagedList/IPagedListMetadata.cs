using System;

namespace NovaPagedList
{
    /// <summary>
    /// Specifies the metadata of the <see cref="IPagedList{T}"/> and its superset list.
    /// </summary>
    public interface IPagedListMetadata
    {
        /// <summary>
        /// Gets the one-based number of the current page.
        /// </summary>
        int PageNumber { get; }

        /// <summary>
        /// Gets the maximum size of a page.
        /// </summary>
        int PageSize { get; }

        /// <summary>
        /// Gets the total number of items in the superset list.
        /// </summary>
        int TotalItemCount { get; }

        /// <summary>
        /// Gets the total number of pages in the superset list.
        /// </summary>
        int PageCount { get; }

        /// <summary>
        /// Gets the number of items on the current page.
        /// </summary>
        int ItemCountOnPage { get; }
    }
}
