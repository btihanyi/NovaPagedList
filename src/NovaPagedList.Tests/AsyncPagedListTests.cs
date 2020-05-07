using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace NovaPagedList.Tests
{
    public class AsyncPagedListTests : ToPagedListTestsBase
    {
        [Theory]
        [TupleData(nameof(GetTestData))]
        public async void AsyncQueryableTest(int pageNumber, int pageSize, bool adjustLastPageWhenExceeding)
        {
            var list = DataSource.AsAsyncQueryable();
            var pagedList = list.ToPagedList(pageNumber, pageSize, adjustLastPageWhenExceeding);
            var asyncPagedList = await list.ToAsyncPagedListAsync(pageNumber, pageSize, adjustLastPageWhenExceeding);

            AssertPagedListMetadaata(asyncPagedList, pageNumber, pageSize, adjustLastPageWhenExceeding);

            var pagedListEnumerator = pagedList.GetEnumerator();
            await foreach (int item in asyncPagedList)
            {
                pagedListEnumerator.MoveNext().Should().BeTrue();
                item.Should().Be(pagedListEnumerator.Current);
            }

            pagedListEnumerator.MoveNext().Should().BeFalse();
        }
    }
}
