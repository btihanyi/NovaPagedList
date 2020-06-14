using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#if !NETSTANDARD2_0
using System.Threading;
using System.Threading.Tasks;
#endif

namespace NovaPagedList
{
    /// <summary>
    /// Represents an empty <see cref="IPagedList{T}"/>.
    /// </summary>
    /// <typeparam name="T">The item type.</typeparam>
    public sealed class EmptyPagedList<T> : IPagedList<T>
#if !NETSTANDARD2_0
        #pragma warning disable SA1001 // Commas should be spaced correctly
        , IAsyncPagedList<T>
        #pragma warning restore SA1001 // Commas should be spaced correctly
#endif
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyPagedList{T}"/> class.
        /// </summary>
        /// <param name="pageSize">The size of a page.</param>
        public EmptyPagedList(int pageSize)
        {
            if (pageSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize), pageSize, "The page size must be positive.");
            }

            this.PageSize = pageSize;
        }

        /// <summary>
        /// Gets the size of a page.
        /// </summary>
        public int PageSize { get; }

        /// <summary>
        /// Gets the total number of items, which is always zero.
        /// </summary>
        public int TotalItemCount => 0;

        /// <summary>
        /// Gets the total number of pages, which is always zero.
        /// </summary>
        public int PageCount => 0;

        /// <summary>
        /// Gets the number of items on the current page, which is always zero.
        /// </summary>
        public int Count => 0;

        /// <summary>
        /// Gets the one-based number of the current page. It always throws an <see cref="InvalidOperationException"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">There are no pages.</exception>
        public int PageNumber => throw new InvalidOperationException("There are no pages.");

        /// <summary>
        /// Gets the number of items on the current page. It always throws an <see cref="InvalidOperationException"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">There are no pages.</exception>
        public int ItemCountOnPage => throw new InvalidOperationException("There are no pages.");

        /// <summary>
        /// The specified item of the current page. It always throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <param name="index">The zero-based index of current page's items.</param>
        /// <returns>Nothing, since it throws an <see cref="ArgumentOutOfRangeException"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">This list is empty.</exception>
        public T this[int index] => throw new ArgumentOutOfRangeException(nameof(index), "This list is empty.");

        /// <summary>
        /// Returns an empty enumerator with no elements.
        /// </summary>
        /// <returns>An empty enumerator.</returns>
        public IEnumerator<T> GetEnumerator() => Enumerable.Empty<T>().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

#if !NETSTANDARD2_0
        /// <summary>
        /// Returns an empty asynchronous enumerator with no elements.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that may be used to
        /// cancel the asynchronous iteration.</param>
        /// <returns>An empty asynchronous enumerator.</returns>
        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return default(EmptyAsyncEnumerator);
        }

        private struct EmptyAsyncEnumerator : IAsyncEnumerator<T>
        {
            public T Current => throw new InvalidOperationException();

            public ValueTask DisposeAsync() => default(ValueTask);

            public ValueTask<bool> MoveNextAsync() => new ValueTask<bool>(false);
        }
#endif
    }
}
