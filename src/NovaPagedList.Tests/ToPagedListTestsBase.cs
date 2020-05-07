using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;

namespace NovaPagedList.Tests
{
    public class ToPagedListTestsBase
    {
        protected const int DataSourceItemCount = 100;
        protected static readonly IEnumerable<int> DataSource = new List<int>(Enumerable.Range(1, DataSourceItemCount));

        public static IEnumerable<(int pageNumber, int pageSize, bool adjustLastPageWhenExceeding)> GetTestData()
        {
            return new[]
            {
                (pageNumber: 1,  pageSize: 1,  adjustLastPageWhenExceeding: true),
                (pageNumber: 1,  pageSize: 10, adjustLastPageWhenExceeding: true),
                (pageNumber: 10, pageSize: 1,  adjustLastPageWhenExceeding: true),
                (pageNumber: 10, pageSize: 10, adjustLastPageWhenExceeding: true),
                (pageNumber: 11, pageSize: 10, adjustLastPageWhenExceeding: true),
                (pageNumber: 11, pageSize: 10, adjustLastPageWhenExceeding: false),
                (pageNumber: 9,  pageSize: 11, adjustLastPageWhenExceeding: true),
                (pageNumber: 10, pageSize: 11, adjustLastPageWhenExceeding: true),
                (pageNumber: 11, pageSize: 11, adjustLastPageWhenExceeding: true),
                (pageNumber: 10, pageSize: 11, adjustLastPageWhenExceeding: false),
                (pageNumber: 10, pageSize: 11, adjustLastPageWhenExceeding: false),
            };
        }

        protected void AssertPagedListMetadaata(IPagedListMetadata metadata, int pageNumber, int pageSize, bool adjustLastPageWhenExceeding)
        {
            metadata.TotalItemCount.Should().Be(DataSourceItemCount);
            metadata.PageSize.Should().Be(pageSize);

            int minNumberOfPages = DataSourceItemCount / pageSize;
            int pageCount = (minNumberOfPages * pageSize == DataSourceItemCount) ? minNumberOfPages : minNumberOfPages + 1;
            metadata.PageCount.Should().Be(pageCount);

            if (adjustLastPageWhenExceeding && pageNumber > pageCount)
            {
                metadata.PageNumber.Should().Be(pageCount);
            }
            else
            {
                metadata.PageNumber.Should().Be(pageNumber);
            }

            if ((metadata.PageNumber - 1) * pageSize >= DataSourceItemCount)
            {
                metadata.ItemCountOnPage.Should().Be(0);
            }
            else if (metadata.PageNumber * pageSize <= DataSourceItemCount)
            {
                metadata.ItemCountOnPage.Should().Be(pageSize);
            }
            else
            {
                metadata.ItemCountOnPage.Should().Be(DataSourceItemCount % pageSize);
            }
        }

        protected void AssertPagedListContent(IPagedList<int> pagedList, int pageNumber, int pageSize, bool adjustLastPageWhenExceeding)
        {
            IEnumerable<int> expectedData;

            do
            {
                expectedData = DataSource.Skip((pageNumber - 1) * pageSize).Take(pageSize);
                pageNumber--;
            }
            while (adjustLastPageWhenExceeding && pageNumber > 0 && !expectedData.Any());

            pagedList.Should().Equal(expectedData);
        }
    }
}
