using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace NovaPagedList
{
    /// <summary>
    /// This is an extension class for Entity Framework 6 users.
    /// </summary>
    public static class EntityFrameworkExtensions
    {
        /// <summary>
        /// Creates a <see cref="IPagedList{T}"/> from the query's result, containing only the specified page's records.
        /// </summary>
        /// <typeparam name="T">The record type of the query.</typeparam>
        /// <param name="superset">The query to be executed.</param>
        /// <param name="pageNumber">The one-based number of the required page.</param>
        /// <param name="pageSize">The size of the pages.</param>
        /// <param name="adjustLastPageWhenExceeding">If <see langword="true"/> and the <paramref name="pageNumber"/> is greater
        /// than the available pages, it automatically adjusts the page number to the last page.</param>
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
    }
}
