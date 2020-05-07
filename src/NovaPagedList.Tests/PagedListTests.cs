using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace NovaPagedList.Tests
{
    public class PagedListTests
    {
        private static IEnumerable<(int pageNumber, int pageSize, int totalItemCount)> GetMetadata()
        {
            return new[]
            {
                (pageNumber: 1,  pageSize: 1,  totalItemCount: 1),
                (pageNumber: 1,  pageSize: 2,  totalItemCount: 3),
                (pageNumber: 10, pageSize: 1,  totalItemCount: 100),
                (pageNumber: 10, pageSize: 10, totalItemCount: 100),
                (pageNumber: 11, pageSize: 10, totalItemCount: 100),
            };
        }

        public static IEnumerable<object[]> GetTestData()
        {
            return GetMetadata().Select(x => new object[]
            {
                new List<int>(Enumerable.Range(((x.pageNumber - 1) * x.pageSize) + 1, x.pageSize)),
                x.pageNumber,
                x.pageSize,
                x.totalItemCount,
            });
        }

        [Theory]
        [MemberData(nameof(GetTestData))]
        public void Read_Only_Auto_Properties_Must_Return_Original_Values(List<int> subset, int pageNumber, int pageSize, int totalItemCount)
        {
            var pagedList = new PagedList<int>(subset, pageNumber, pageSize, totalItemCount);

            pagedList.PageNumber.Should().Be(pageNumber);
            pagedList.PageSize.Should().Be(pageSize);
            pagedList.TotalItemCount.Should().Be(totalItemCount);
            pagedList.Count.Should().Be(subset.Count);
        }

        [Theory]
        [MemberData(nameof(GetTestData))]
        public void PageCount_Should_Be_Calculated_Properly(List<int> subset, int pageNumber, int pageSize, int totalItemCount)
        {
            var pagedList = new PagedList<int>(subset, pageNumber, pageSize, totalItemCount);

            int minPageCount = totalItemCount / pageSize;
            int pageCount = (minPageCount * pageSize == totalItemCount) ? minPageCount : minPageCount + 1;
            pagedList.PageCount.Should().Be(pageCount);
        }

        [Theory]
        [MemberData(nameof(GetTestData))]
        public void PagedList_Items_Should_Be_The_Original_Subset(List<int> subset, int pageNumber, int pageSize, int totalItemCount)
        {
            var pagedList = new PagedList<int>(subset, pageNumber, pageSize, totalItemCount);

            // Checking enumerators
            pagedList.Should().Equal(subset);

            // Checking indexers
            for (int i = 0; i < subset.Count; i++)
            {
                pagedList[i].Should().Be(subset[i]);
            }
        }

        [Fact]
        public void Null_Parameter_Should_Throw_Exception()
        {
            FluentActions.Invoking(() => new PagedList<int>(null, 1, 1, 1)).Should().Throw<NullReferenceException>();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Page_Number_Must_Be_Positive(int pageNumber)
        {
            var list = new List<int>() { 1 };
            FluentActions.Invoking(() => new PagedList<int>(list, pageNumber, 1, 1)).Should().Throw<ArgumentOutOfRangeException>();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Page_Size_Must_Be_Positive(int pageSize)
        {
            var list = new List<int>() { 1 };
            FluentActions.Invoking(() => new PagedList<int>(list, 1, pageSize, 1)).Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void Total_Item_Count_Must_Be_Nonnegative()
        {
            FluentActions.Invoking(() => new PagedList<int>(Array.Empty<int>(), 1, 1, totalItemCount: -1))
                         .Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void Total_Item_Count_Can_Be_Zero()
        {
            FluentActions.Invoking(() => new PagedList<int>(Array.Empty<int>(), 1, 1, totalItemCount: 0))
                         .Should().NotThrow<ArgumentOutOfRangeException>();
        }

        [Theory]
        [InlineData(10)]
        public void Subset_Cannot_Contain_More_Items_Than_The_Page_Size(int pageSize)
        {
            var list = new List<int>(Enumerable.Range(1, pageSize));
            FluentActions.Invoking(() => new PagedList<int>(list, pageNumber: 1, pageSize, totalItemCount: pageSize))
                         .Should().NotThrow<ArgumentOutOfRangeException>();

            list.Add(1);
            FluentActions.Invoking(() => new PagedList<int>(list, pageNumber: 1, pageSize, totalItemCount: pageSize))
                         .Should().Throw<ArgumentOutOfRangeException>();
        }

        [Theory]
        [InlineData(10)]
        public void Subset_Cannot_Contain_More_Items_Than_The_Total_Item_Count(int totalItemCount)
        {
            var list = new List<int>(Enumerable.Range(1, totalItemCount));
            FluentActions.Invoking(() => new PagedList<int>(list, pageNumber: 1, pageSize: list.Count, totalItemCount))
                         .Should().NotThrow<ArgumentOutOfRangeException>();

            list.Add(1);
            FluentActions.Invoking(() => new PagedList<int>(list, pageNumber: 1, pageSize: list.Count, totalItemCount))
                         .Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}
