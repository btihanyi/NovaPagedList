using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace NovaPagedList.Tests
{
    public class PartitionTests
    {
        [Fact]
        public void Empty_Collection_Returns_No_Subsets()
        {
            var emptyList = Enumerable.Empty<int>().Unrepeatable();

            emptyList.Partition(10).Should().BeEmpty();
        }

        [Theory]
        [InlineData(1, 9)]
        [InlineData(3, 9)]
        [InlineData(4, 9)]
        [InlineData(5, 9)]
        [InlineData(9, 9)]
        [InlineData(10, 9)]
        public void Partitions_Contain_Expected_Subsets(int pageSize, int totalItemCount)
        {
            var list = Enumerable.Range(1, totalItemCount).Unrepeatable();
            var partitionList = Enumerable.Range(1, totalItemCount).Partition(pageSize).ToList();

            // Test the whole list
            partitionList.Should().HaveCount((int) Math.Ceiling((double) totalItemCount / pageSize));
            partitionList.SelectMany(x => x).Should().Equal(list);

            // Test each subsets
            for (int i = 0; i < partitionList.Count; i++)
            {
                var partition = partitionList[i];

                if (i < partitionList.Count - 1 || totalItemCount % pageSize == 0)
                {
                    partition.Should().HaveCount(pageSize);
                }
                else
                {
                    partition.Should().HaveCount(totalItemCount % pageSize);
                }

                partition.PageNumber.Should().Be(i + 1);
                partition.PageSize.Should().Be(pageSize);
                partition.TotalItemCount.Should().Be(totalItemCount);
                partition.PageCount.Should().Be(partitionList.Count);
            }
        }

        [Fact]
        public void Null_Collection_Throws_Exception()
        {
            ((IEnumerable<int>) null).Invoking(x => x.Partition(10))
                                     .Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(0, 10)]
        [InlineData(-1, 0)]
        [InlineData(-1, 10)]
        public void Invalid_Page_Size_Throws_Exception(int pageSize, int totalItemCount)
        {
            Enumerable.Range(1, totalItemCount)
                      .Invoking(x => x.Partition(pageSize))
                      .Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}
