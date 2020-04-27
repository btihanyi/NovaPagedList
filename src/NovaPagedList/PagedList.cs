using System;
using System.Collections;
using System.Collections.Generic;

namespace NovaPagedList
{
    /// <summary>
    /// Represents a concrete subset (page) of the superset.
    /// </summary>
    /// <typeparam name="T">The item type.</typeparam>
    public class PagedList<T> : IPagedList<T>
    {
        private readonly IReadOnlyList<T> items;

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedList{T}"/> class.
        /// </summary>
        /// <param name="subset">The subset items of the superset.</param>
        /// <param name="pageNumber">The one-based number of the current page.</param>
        /// <param name="pageSize">The maximum size of a page.</param>
        /// <param name="totalItemCount">The total number of items in the superset list.</param>
        public PagedList(IReadOnlyList<T> subset, int pageNumber, int pageSize, int totalItemCount)
        {
            if (pageNumber <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageNumber), pageNumber, "The page number must be positive.");
            }
            if (pageSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize), pageSize, "The page size must be positive.");
            }
            if (totalItemCount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(totalItemCount), totalItemCount, "The total item count cannot be negative.");
            }
            if (subset.Count > pageSize)
            {
                throw new ArgumentException(nameof(subset), "The subset's size cannot be larger than the page size.");
            }

            this.items = subset ?? throw new ArgumentNullException(nameof(subset));

            this.PageSize = pageSize;
            this.TotalItemCount = totalItemCount;
            this.PageCount = (int) Math.Ceiling((double) TotalItemCount / PageSize);
            this.PageNumber = Math.Min(pageNumber, PageCount);
        }

        /// <summary>
        /// The one-based number of the current page.
        /// </summary>
        public int PageNumber { get; }

        /// <summary>
        /// The maximum size of a page.
        /// </summary>
        public int PageSize { get; }

        /// <summary>
        /// The total number of items in the superset list.
        /// </summary>
        public int TotalItemCount { get; }

        /// <summary>
        /// The total number of pages in the superset list.
        /// </summary>
        public int PageCount { get; }

        /// <summary>
        /// The specified item of the current page.
        /// </summary>
        /// <param name="index">The zero-based index of current page's items.</param>
        /// <returns>The item at the specified index.</returns>
        public T this[int index] => items[index];

        /// <summary>
        /// The number of items on the current page.
        /// </summary>
        public int Count => items.Count;

        int IPagedListMetadata.ItemCountOnPage => items.Count;

        /// <summary>
        /// Returns an enumerator that iterates through the current page's items.
        /// </summary>
        /// <returns>An enumerator for page's items.</returns>
        public IEnumerator<T> GetEnumerator() => items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
