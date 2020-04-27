using System;
using System.Collections.Generic;
using System.Linq;

namespace NovaPagedList
{
    /// <summary>
    /// Extension class for creating subset instances from different kind of supersets.
    /// </summary>
    public static class PagedListExtensions
    {
        private static IEnumerable<T> CreateSubset<T>(IEnumerable<T> superset, int pageNumber, int pageSize,
            int totalItemCount, bool adjustLastPageWhenExceeding)
        {
            if (adjustLastPageWhenExceeding)
            {
                int pageCount = (int) Math.Ceiling((double) totalItemCount / pageSize);
                if (pageNumber > pageCount)
                {
                    pageNumber = pageCount;
                }
            }

            var subset = superset;
            if (pageNumber > 1)
            {
                subset = subset.Skip((pageNumber - 1) * pageSize);
            }

            return subset.Take(pageSize);
        }

        /// <summary>
        /// Creates an <see cref="IPagedList{T}"/> subset from an <see cref="IEnumerable{T}"/> <paramref name="superset"/>.
        /// The <see cref="PagedList{T}.TotalItemCount"/> must be calculated first by calling the
        /// <see cref="Enumerable.Count{TSource}(IEnumerable{TSource})"/> method on the <paramref name="superset"/>.
        /// </summary>
        /// <typeparam name="T">The item type.</typeparam>
        /// <param name="superset">The whole collection of all the items, which can be an <see cref="IQueryable{T}"/> data source.</param>
        /// <param name="pageNumber">The one-based number of the required page.</param>
        /// <param name="pageSize">The size of the pages.</param>
        /// <param name="adjustLastPageWhenExceeding">If <see langword="true"/> and the <paramref name="pageNumber"/>
        /// is greater than the available pages, it automatically adjusts the page number to the last page.</param>
        /// <returns>A <see cref="PagedList{T}"/> instance if there are available items, or an <see cref="EmptyPagedList{T}"/>
        /// instance when the superset is empty.</returns>
        public static IPagedList<T> ToPagedList<T>(this IEnumerable<T> superset, int pageNumber, int pageSize,
            bool adjustLastPageWhenExceeding = true)
        {
            if (superset == null)
            {
                throw new ArgumentNullException(nameof(superset));
            }
            if (pageNumber <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageNumber), pageNumber, "The page number must be positive.");
            }
            if (pageSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize), pageSize, "The page size must be positive.");
            }

            int totalItemCount = superset.Count();
            if (totalItemCount > 0)
            {
                var subset = CreateSubset(superset, pageNumber, pageSize, totalItemCount, adjustLastPageWhenExceeding);
                return new PagedList<T>(subset.ToList(), pageNumber, pageSize, totalItemCount);
            }
            else
            {
                return new EmptyPagedList<T>(pageSize);
            }
        }

        /// <summary>
        /// Creates an <see cref="IPagedList{T}"/> subset from an <see cref="IReadOnlyCollection{T}"/> <paramref name="superset"/>.
        /// </summary>
        /// <typeparam name="T">The item type.</typeparam>
        /// <param name="superset">The whole collection of all the items.</param>
        /// <param name="pageNumber">The one-based number of the required page.</param>
        /// <param name="pageSize">The size of the pages.</param>
        /// <param name="adjustLastPageWhenExceeding">If <see langword="true"/> and the <paramref name="pageNumber"/>
        /// is greater than the available pages, it automatically adjusts the page number to the last page.</param>
        /// <returns>A <see cref="PagedList{T}"/> instance if there are available items, or an <see cref="EmptyPagedList{T}"/>
        /// instance when the superset is empty.</returns>
        public static IPagedList<T> ToPagedList<T>(this IReadOnlyCollection<T> superset, int pageNumber, int pageSize,
            bool adjustLastPageWhenExceeding = true)
        {
            if (superset == null)
            {
                throw new ArgumentNullException(nameof(superset));
            }
            if (pageNumber <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageNumber), pageNumber, "The page number must be positive.");
            }
            if (pageSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize), pageSize, "The page size must be positive.");
            }

            int totalItemCount = superset.Count;
            if (totalItemCount > 0)
            {
                var subset = CreateSubset(superset, pageNumber, pageSize, totalItemCount, adjustLastPageWhenExceeding);
                return new PagedList<T>(subset.ToList(), pageNumber, pageSize, totalItemCount);
            }
            else
            {
                return new EmptyPagedList<T>(pageSize);
            }
        }


        /// <summary>
        /// Creates an <see cref="IPagedList{T}"/> subset from an <see cref="IQueryable{T}"/> <paramref name="superset"/>.
        /// </summary>
        /// <typeparam name="T">The record type of the query.</typeparam>
        /// <param name="superset">The query to be converted to a subset query.</param>
        /// <param name="pageNumber">The one-based number of the required page. If <paramref name="adjustLastPageWhenExceeding"/>
        /// is <see langword="true"/> and <paramref name="pageNumber"/> is greater than the available pages, then it will be
        /// set to the number of the last page.</param>
        /// <param name="pageSize">The size of the pages.</param>
        /// <param name="adjustLastPageWhenExceeding">If <see langword="true"/> and the <paramref name="pageNumber"/>
        /// is greater than the available pages, it automatically adjusts the page number to the last page.</param>
        /// <returns>A <see cref="PagedList{T}"/> instance if there are available records, or an <see cref="EmptyPagedList{T}"/>
        /// instance when the <paramref name="superset"/> is empty.</returns>
        public static IPagedList<T> ToPagedList<T>(this IQueryable<T> superset, int pageNumber, int pageSize,
            bool adjustLastPageWhenExceeding = true)
        {
            if (superset == null)
            {
                throw new ArgumentNullException(nameof(superset));
            }
            if (pageNumber <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageNumber), pageNumber, "The page number must be positive.");
            }
            if (pageSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize), pageSize, "The page size must be positive.");
            }

            int totalItemCount = superset.Count();
            if (totalItemCount > 0)
            {
                if (adjustLastPageWhenExceeding)
                {
                    int pageCount = (int) Math.Ceiling((double) totalItemCount / pageSize);
                    if (pageNumber > pageCount)
                    {
                        pageNumber = pageCount;
                    }
                }

                var subset = superset;
                if (pageNumber > 1)
                {
                    subset = subset.Skip((pageNumber - 1) * pageSize);
                }

                subset = subset.Take(pageSize);

                return new PagedList<T>(subset.ToList(), pageNumber, pageSize, totalItemCount);
            }
            else
            {
                return new EmptyPagedList<T>(pageSize);
            }
        }

        /// <summary>
        /// Creates a new query from the <paramref name="superset"/> without actually executing it. If the <paramref name="totalItemCount"/>
        /// is not known, use the <see cref="ToPagedQueryable{T}(IQueryable{T}, ref int, int, out int, bool)"/> method instead.
        /// </summary>
        /// <typeparam name="T">The record type of the query.</typeparam>
        /// <param name="superset">The query to be converted to a subset query.</param>
        /// <param name="pageNumber">The one-based number of the required page. If <paramref name="adjustLastPageWhenExceeding"/> is
        /// <see langword="true"/> and <paramref name="pageNumber"/> is greater than the available pages, then it will be set to the
        /// number of the last page.</param>
        /// <param name="pageSize">The size of the pages.</param>
        /// <param name="totalItemCount">The total number of the records in the data source. If it is not known, use the
        /// <see cref="ToPagedQueryable{T}(IQueryable{T}, ref int, int, out int, bool)"/> method instead.</param>
        /// <param name="adjustLastPageWhenExceeding">If <see langword="true"/> and the <paramref name="pageNumber"/>
        /// is greater than the available pages, it automatically adjusts the page number to the last page.</param>
        /// <returns>The query converted to the subset of the <paramref name="superset"/>.</returns>
        public static IQueryable<T> ToPagedQueryable<T>(IQueryable<T> superset, ref int pageNumber, int pageSize, int totalItemCount,
            bool adjustLastPageWhenExceeding = true)
        {
            if (superset == null)
            {
                throw new ArgumentNullException(nameof(superset));
            }
            if (pageNumber <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageNumber), pageNumber, "The page number must be positive.");
            }
            if (pageSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize), pageSize, "The page size must be positive.");
            }

            if (adjustLastPageWhenExceeding)
            {
                int pageCount = (int) Math.Ceiling((double) totalItemCount / pageSize);
                if (pageNumber > pageCount)
                {
                    pageNumber = pageCount;
                }
            }

            var subset = superset;
            if (pageNumber > 1)
            {
                subset = subset.Skip((pageNumber - 1) * pageSize);
            }

            return subset.Take(pageSize);
        }

        /// <summary>
        /// Creates a new query from the <paramref name="superset"/>. The <paramref name="totalItemCount"/> must be calculated,
        /// hence an <see cref="Queryable.Count{TSource}(IQueryable{TSource})"/> will be executed upon calling this method.
        /// </summary>
        /// <typeparam name="T">The record type of the query.</typeparam>
        /// <param name="superset">The query to be converted to a subset query.</param>
        /// <param name="pageNumber">The one-based number of the required page. If <paramref name="adjustLastPageWhenExceeding"/> is
        /// <see langword="true"/> and <paramref name="pageNumber"/> is greater than the available pages, then it will be set to the
        /// number of the last page.</param>
        /// <param name="pageSize">The size of the pages.</param>
        /// <param name="totalItemCount">The total number of the records in the data source. It will be calculated by
        /// calling the <see cref="Queryable.Count{TSource}(IQueryable{TSource})"/> method.</param>
        /// <param name="adjustLastPageWhenExceeding">If <see langword="true"/> and the <paramref name="pageNumber"/>
        /// is greater than the available pages, it automatically adjusts the page number to the last page.</param>
        /// <returns>The query converted to the subset of the <paramref name="superset"/>.</returns>
        public static IQueryable<T> ToPagedQueryable<T>(IQueryable<T> superset, ref int pageNumber, int pageSize, out int totalItemCount,
            bool adjustLastPageWhenExceeding = true)
        {
            totalItemCount = superset.Count();
            return ToPagedQueryable(superset, ref pageNumber, pageSize, totalItemCount, adjustLastPageWhenExceeding);
        }

        /// <summary>
        /// Creates a list of multiple <see cref="IPagedList{T}"/> instances by splitting the superset various pages.
        /// The <paramref name="superset"/> will be enumerated only once.
        /// </summary>
        /// <typeparam name="T">The item type.</typeparam>
        /// <param name="superset">The whole collection of all the items.</param>
        /// <param name="pageSize">The size of the pages.</param>
        /// <returns>⌈&lt;total item count&gt; / <paramref name="pageSize"/>⌉ number of <see cref="IPagedList{T}"/> instances.</returns>
        public static IReadOnlyList<IPagedList<T>> Partition<T>(this IEnumerable<T> superset, int pageSize)
        {
            if (superset == null)
            {
                throw new ArgumentNullException(nameof(superset));
            }
            if (pageSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize), "The page size must be positive.");
            }

            int totalItemCount = 0;
            var subsets = new List<List<T>>();
            List<T>? subset = null;

            var enumerator = superset.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (subset == null || subset.Count == pageSize)
                {
                    subset = new List<T>(pageSize);
                    subsets.Add(subset);
                }

                subset.Add(enumerator.Current);
                totalItemCount++;
            }

            var result = new List<IPagedList<T>>(subsets.Count);
            result.AddRange(subsets.Select((subset, index) => new PagedList<T>(subset, pageNumber: index + 1, pageSize, totalItemCount)));
            return result;
        }
    }
}
