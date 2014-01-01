using System.Collections.Generic;
using Nelibur.Core.Extensions;
using Xunit;

namespace UnitTests.Nelibur.Core.Extensions
{
    public sealed class CollectionExtensionsTests
    {
        [Fact]
        public void IsEmpty_Empty_True()
        {
            bool result = new List<int>().IsEmpty();
            Assert.True(result);
        }

        [Fact]
        public void IsNotEmpty_NotEmpty_True()
        {
            bool result = new List<int> { 1 }.IsNotEmpty();
            Assert.True(result);
        }

        [Fact]
        public void IsNullOrEmptyNull_Null_True()
        {
            bool result = ((ICollection<int>)null).IsNullOrEmpty();
            Assert.True(result);
        }

        [Fact]
        public void IsNullOrEmpty_Empty_True()
        {
            bool result = new List<int>().IsNullOrEmpty();
            Assert.True(result);
        }

        [Fact]
        public void IsNullOrEmpty_NotEmpty_False()
        {
            bool result = new List<int> { 1 }.IsNullOrEmpty();
            Assert.False(result);
        }
    }
}
