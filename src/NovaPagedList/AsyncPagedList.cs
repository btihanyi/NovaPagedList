#if !NETSTANDARD2_0
using System;
using System.Collections.Generic;
using System.Threading;

namespace NovaPagedList
{
    /// <summary>
    /// Represents an asynchronously enumerable subset (page) of the superset.
    /// </summary>
    /// <typeparam name="T">The item type.</typeparam>
    public class AsyncPagedList<T> : IAsyncPagedList<T>
    {
        private readonly IAsyncEnumerable<T> items;
        private List<T>? cachedItems;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncPagedList{T}"/> class.
        /// </summary>
        /// <param name="subset">The asynchronously enumerable subset items of the superset.</param>
        /// <param name="count">The number of available items in the current <paramref name="subset"/>.</param>
        /// <param name="pageNumber">The one-based number of the current page.</param>
        /// <param name="pageSize">The maximum size of a page.</param>
        /// <param name="totalItemCount">The total number of items in the superset list.</param>
        /// <param name="cacheItems">If <see langword="true"/>, the <paramref name="subset"/> will be cached for repeated listings.</param>
        public AsyncPagedList(IAsyncEnumerable<T> subset, int count, int pageNumber, int pageSize, int totalItemCount, bool cacheItems = false)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count), pageNumber, "The subset's size must be nonnegative.");
            }
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
            if (count > pageSize)
            {
                throw new ArgumentException(nameof(subset), "The subset's size cannot be larger than the page size.");
            }

            this.items = subset ?? throw new ArgumentNullException(nameof(subset));
            this.PageNumber = pageNumber;
            this.ItemCountOnPage = count;
            this.PageSize = pageSize;
            this.TotalItemCount = totalItemCount;
            this.PageCount = (int) Math.Ceiling((double) TotalItemCount / PageSize);
            this.CacheItems = cacheItems;
        }

        /// <summary>
        /// Gets the one-based number of the current page.
        /// </summary>
        public int PageNumber { get; }

        /// <summary>
        /// Gets the maximum size of a page.
        /// </summary>
        public int PageSize { get; }

        /// <summary>
        /// Gets the total number of items in the superset list.
        /// </summary>
        public int TotalItemCount { get; }

        /// <summary>
        /// Gets the total number of pages in the superset list.
        /// </summary>
        public int PageCount { get; }

        /// <summary>
        /// Gets the number of items on the current page.
        /// </summary>
        public int ItemCountOnPage { get; }

        /// <summary>
        /// Gets a value indicating whether the enumerated items are cached for repeated listings.
        /// </summary>
        public bool CacheItems { get; }

        /// <summary>
        /// Returns an enumerator that iterates asynchronously through the page's items.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that may be used to
        /// cancel the asynchronous iteration.</param>
        /// <returns>An enumerator that can be used to iterate asynchronously through the page's items.</returns>
        public async IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            if (CacheItems)
            {
                if (cachedItems != null)
                {
                    foreach (var item in cachedItems)
                    {
                        yield return item;
                    }
                }
                else
                {
                    var list = new List<T>(TotalItemCount);
                    var enumerator = items.GetAsyncEnumerator(cancellationToken);

                    try
                    {
                        while (await enumerator.MoveNextAsync())
                        {
                            var item = enumerator.Current;
                            yield return item;
                            list.Add(item);
                        }
                    }
                    finally
                    {
                        await enumerator.DisposeAsync();
                    }

                    cachedItems = list;
                }
            }
            else
            {
                var enumerator = items.GetAsyncEnumerator(cancellationToken);

                try
                {
                    while (await enumerator.MoveNextAsync())
                    {
                        yield return enumerator.Current;
                    }
                }
                finally
                {
                    await enumerator.DisposeAsync();
                }
            }
        }
    }
}
#endif
