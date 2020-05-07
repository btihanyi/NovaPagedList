using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace NovaPagedList.Tests
{
    public class MetadataExtensionsTests
    {
        [Theory]
        [InlineData(1, 5, true)]
        [InlineData(2, 5, false)]
        [InlineData(5, 5, false)]
        [InlineData(0, 5, false)]
        [InlineData(1, 1, true)]
        [InlineData(0, 0, false)]
        public void IsFirstPage_Should_Be_True_When_Page_Number_Is_One(int pageNumber, int pageCount, bool isFirstPage)
        {
            var metadata = new PagedListMetadataStub(pageNumber, pageCount);
            metadata.IsFirstPage().Should().Be(isFirstPage);
        }

        [Theory]
        [InlineData(1, 5, false)]
        [InlineData(2, 5, false)]
        [InlineData(5, 5, true)]
        [InlineData(0, 5, false)]
        [InlineData(1, 1, true)]
        [InlineData(0, 0, false)]
        public void IsLastPage_Should_Be_True_When_Page_Number_Is_Total_Count_Of_Pages(int pageNumber, int pageCount, bool isLastPage)
        {
            var metadata = new PagedListMetadataStub(pageNumber, pageCount);
            metadata.IsLastPage().Should().Be(isLastPage);
        }

        [Theory]
        [InlineData(1, 5, true)]
        [InlineData(2, 5, true)]
        [InlineData(5, 5, false)]
        [InlineData(0, 5, true)]
        [InlineData(1, 1, false)]
        [InlineData(0, 0, false)]
        public void HasNextPage_Should_Be_True_When_Page_Number_Is_Not_The_Last_One(int pageNumber, int pageCount, bool hasNextPage)
        {
            var metadata = new PagedListMetadataStub(pageNumber, pageCount);
            metadata.HasNextPage().Should().Be(hasNextPage);
        }

        [Theory]
        [InlineData(1, 5, false)]
        [InlineData(2, 5, true)]
        [InlineData(5, 5, true)]
        [InlineData(0, 5, false)]
        [InlineData(1, 1, false)]
        [InlineData(0, 0, false)]
        [InlineData(9, 5, true)]
        [InlineData(9, 0, false)]
        public void HasPreviousPage_Should_Be_True_When_Page_Number_Greater_Than_One(int pageNumber, int pageCount, bool hasPreviousPage)
        {
            var metadata = new PagedListMetadataStub(pageNumber, pageCount);
            metadata.HasPreviousPage().Should().Be(hasPreviousPage);
        }

        private class PagedListMetadataStub : IPagedListMetadata
        {
            public PagedListMetadataStub(int pageNumber, int pageCount)
            {
                this.PageNumber = pageNumber;
                this.PageCount = pageCount;
            }

            public int PageNumber { get; }

            public int PageCount { get; }

            public int PageSize => throw new NotImplementedException();

            public int TotalItemCount => throw new NotImplementedException();

            public int ItemCountOnPage => throw new NotImplementedException();
        }
    }
}
