using System;

namespace NovaPagedList
{
    /// <summary>
    /// Specifies the metadata of the <see cref="IPagedList{T}"/> and its superset list.
    /// </summary>
    public interface IPagedListMetadata
    {
        /// <summary>
        /// The one-based number of the current page.
        /// </summary>
        int PageNumber { get; }

        /// <summary>
        /// The maximum size of a page.
        /// </summary>
        int PageSize { get; }

        /// <summary>
        /// The total number of items in the superset list.
        /// </summary>
        int TotalItemCount { get; }

        /// <summary>
        /// The total number of pages in the superset list.
        /// </summary>
        int PageCount { get; }

        /// <summary>
        /// The number of items on the current page.
        /// </summary>
        int ItemCountOnPage { get; }
    }
}
