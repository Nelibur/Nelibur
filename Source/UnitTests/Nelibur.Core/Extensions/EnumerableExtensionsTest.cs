using System.Collections.Generic;
using System.Linq;
using Nelibur.Core.DataStructures;
using Nelibur.Core.Extensions;
using Xunit;

namespace UnitTests.Nelibur.Core.Extensions
{
    public sealed class EnumerableExtensionsTest
    {
        [Fact]
        public void IsNullOrEmpty_Empty_True()
        {
            bool result = new List<int>().IsNullOrEmpty();
            Assert.True(result);
        }

        [Fact]
        public void IsNullOrEmpty_Null_True()
        {
            bool result = ((IEnumerable<int>)null).IsNullOrEmpty();
            Assert.True(result);
        }

        [Fact]
        public void ToValue_EmptyBag_Empty()
        {
            IEnumerable<int> result = new List<Option<int>> { Option<int>.Empty }.ToValue();
            Assert.True(!result.Any());
        }

        [Fact]
        public void ToValue_NotEmptyBag_Empty()
        {
            IEnumerable<int> result = new List<Option<int>> { new Option<int>(1) }.ToValue();
            Assert.True(result.Any());
        }
    }
}
