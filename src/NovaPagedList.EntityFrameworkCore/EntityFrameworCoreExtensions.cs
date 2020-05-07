using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace NovaPagedList
{
    /// <summary>
    /// This is an extension class for Entity Framework Core users.
    /// </summary>
    public static class EntityFrameworCoreExtensions
    {
        /// <summary>
        /// Creates a <see cref="IPagedList{T}"/> from the query's result, containing only the specified page's records.
        /// </summary>
        /// <typeparam name="T">The record type of the query.</typeparam>
        /// <param name="superset">The query to be executed.</param>
        /// <param name="pageNumber">The one-based number of the required page.</param>
        /// <param name="pageSize">The size of the pages.</param>
        /// <param name="adjustLastPageWhenExceeding">If <see langword="true"/> and the <paramref name="pageNumber"/>
        /// is greater than the available pages, it automatically adjusts the page number to the last page.</param>
        /// <returns>The specified subset of the <paramref name="superset"/>.</returns>
        public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T> superset, int pageNumber, int pageSize,
            bool adjustLastPageWhenExceeding = true)
        {
            if (superset is null)
            {
                throw new ArgumentNullException(nameof(superset));
            }

            int totalItemCount = await superset.CountAsync();
            var subset = superset.ToPagedQueryable(ref pageNumber, pageSize, totalItemCount, adjustLastPageWhenExceeding);

            if (totalItemCount > 0)
            {
                return new PagedList<T>(await subset.ToListAsync(), pageNumber, pageSize, totalItemCount);
            }
            else
            {
                return new EmptyPagedList<T>(pageSize);
            }
        }

#if !NETSTANDARD2_0
        /// <summary>
        /// Creates a <see cref="IAsyncPagedList{T}"/> from the query's result, waiting only for the specified page's records.
        /// This means it doesn't executes the <paramref name="superset"/> query, but creates an <see cref="IAsyncEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The record type of the query.</typeparam>
        /// <param name="superset">The query to be executed later asynchronously.</param>
        /// <param name="pageNumber">The one-based number of the required page.</param>
        /// <param name="pageSize">The size of the pages.</param>
        /// <param name="adjustLastPageWhenExceeding">If <see langword="true"/> and the <paramref name="pageNumber"/>
        /// is greater than the available pages, it automatically adjusts the page number to the last page.</param>
        /// <param name="cacheItems">If <see langword="true"/>, the query's result will be cached for repeated listings.</param>
        /// <returns>The specified subset of the <paramref name="superset"/> to be queried later asynchronously.</returns>
        public static async Task<IAsyncPagedList<T>> ToAsyncPagedListAsync<T>(this IQueryable<T> superset, int pageNumber, int pageSize,
            bool adjustLastPageWhenExceeding = true, bool cacheItems = false)
        {
            if (superset is null)
            {
                throw new ArgumentNullException(nameof(superset));
            }

            int totalItemCount = await superset.CountAsync();
            var subset = superset.ToPagedQueryable(ref pageNumber, pageSize, totalItemCount, adjustLastPageWhenExceeding);

            if (totalItemCount > 0)
            {
                bool isLastPage = (totalItemCount > (pageNumber - 1) * pageSize) && (totalItemCount <= pageNumber * pageSize);
                int count = isLastPage ? (((totalItemCount - 1) % pageSize) + 1) : pageSize;

                if (!adjustLastPageWhenExceeding && totalItemCount <= (pageNumber - 1) * pageSize)
                {
                    count = 0;
                }

                return new AsyncPagedList<T>(subset.AsAsyncEnumerable(), count, pageNumber, pageSize, totalItemCount, cacheItems);
            }
            else
            {
                return new EmptyPagedList<T>(pageSize);
            }
        }
#endif
    }
}
