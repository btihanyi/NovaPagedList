using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace NovaPagedList.Tests
{
    public class ToPagedListTests : ToPagedListTestsBase
    {
        [Theory]
        [TupleData(nameof(GetTestData))]
        public void Enumerable_ToPageListed_Properly(int pageNumber, int pageSize, bool adjustLastPageWhenExceeding)
        {
            var pagedList = DataSource.Unrepeatable().ToPagedList(pageNumber, pageSize, adjustLastPageWhenExceeding);

            AssertPagedListMetadaata(pagedList, pageNumber, pageSize, adjustLastPageWhenExceeding);
            AssertPagedListContent(pagedList, pageNumber, pageSize, adjustLastPageWhenExceeding);
        }

        [Theory]
        [TupleData(nameof(GetTestData))]
        public void Collection_ToPageListed_Properly(int pageNumber, int pageSize, bool adjustLastPageWhenExceeding)
        {
            IReadOnlyCollection<int> collection = DataSource.ToList();
            var pagedList = collection.ToPagedList(pageNumber, pageSize, adjustLastPageWhenExceeding);

            AssertPagedListMetadaata(pagedList, pageNumber, pageSize, adjustLastPageWhenExceeding);
            AssertPagedListContent(pagedList, pageNumber, pageSize, adjustLastPageWhenExceeding);
        }

        [Theory]
        [TupleData(nameof(GetTestData))]
        public void IReadOnlyList_ToPageListed_Properly(int pageNumber, int pageSize, bool adjustLastPageWhenExceeding)
        {
            IReadOnlyList<int> list = DataSource.ToList();
            var pagedList = list.ToPagedList(pageNumber, pageSize, adjustLastPageWhenExceeding);

            AssertPagedListMetadaata(pagedList, pageNumber, pageSize, adjustLastPageWhenExceeding);
            AssertPagedListContent(pagedList, pageNumber, pageSize, adjustLastPageWhenExceeding);
        }

        [Theory]
        [TupleData(nameof(GetTestData))]
        public void List_ToPageListed_Properly(int pageNumber, int pageSize, bool adjustLastPageWhenExceeding)
        {
            var list = DataSource.ToList();
            var pagedList = list.ToPagedList(pageNumber, pageSize, adjustLastPageWhenExceeding);

            AssertPagedListMetadaata(pagedList, pageNumber, pageSize, adjustLastPageWhenExceeding);
            AssertPagedListContent(pagedList, pageNumber, pageSize, adjustLastPageWhenExceeding);
        }

        [Theory]
        [TupleData(nameof(GetTestData))]
        public void Array_ToPageListed_Properly(int pageNumber, int pageSize, bool adjustLastPageWhenExceeding)
        {
            int[] array = DataSource.ToArray();
            var pagedList = array.ToPagedList(pageNumber, pageSize, adjustLastPageWhenExceeding);

            AssertPagedListMetadaata(pagedList, pageNumber, pageSize, adjustLastPageWhenExceeding);
            AssertPagedListContent(pagedList, pageNumber, pageSize, adjustLastPageWhenExceeding);
        }

        [Theory]
        [TupleData(nameof(GetTestData))]
        public void Queryable_ToPageListed_Properly(int pageNumber, int pageSize, bool adjustLastPageWhenExceeding)
        {
            var queryable = DataSource.AsQueryable();
            var pagedList = queryable.ToPagedList(pageNumber, pageSize, adjustLastPageWhenExceeding);

            AssertPagedListMetadaata(pagedList, pageNumber, pageSize, adjustLastPageWhenExceeding);
            AssertPagedListContent(pagedList, pageNumber, pageSize, adjustLastPageWhenExceeding);
        }
    }
}
