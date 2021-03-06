﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace NovaPagedList.Tests
{
    public class AsyncQueryable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryProvider, IAsyncQueryProvider
    {
        public AsyncQueryable(IEnumerable<T> enumerable)
            : base(enumerable)
        {
        }

        private AsyncQueryable(Expression expression)
            : base(expression)
        {
        }

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return new AsyncEnumerator(this);
        }

        [SuppressMessage("Naming", "RCS1047:Non-asynchronous method name should not end with 'Async'.", Justification = "This is not a real asyn method.")]
        TResult IAsyncQueryProvider.ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            return (TResult) (object) Task.FromResult((T) ((IQueryProvider) this).Execute(expression));
        }

        IQueryable IQueryProvider.CreateQuery(Expression expression)
        {
            throw new NotSupportedException();
        }

        IQueryable<TElement> IQueryProvider.CreateQuery<TElement>(Expression expression)
        {
            return new AsyncQueryable<TElement>(expression);
        }

        private struct AsyncEnumerator : IAsyncEnumerator<T>
        {
            private readonly IEnumerator<T> enumerator;

            public AsyncEnumerator(IEnumerable<T> enumerable)
            {
                this.enumerator = enumerable.GetEnumerator();
            }

            public T Current => enumerator.Current;

            public ValueTask<bool> MoveNextAsync() => new ValueTask<bool>(enumerator.MoveNext());

            public ValueTask DisposeAsync()
            {
                enumerator.Dispose();
                return default(ValueTask);
            }
        }
    }

    public static class AsyncEnumerableExtensions
    {
        public static IQueryable<T> AsAsyncQueryable<T>(this IEnumerable<T> enumerable)
        {
            return new AsyncQueryable<T>(enumerable);
        }
    }
}
