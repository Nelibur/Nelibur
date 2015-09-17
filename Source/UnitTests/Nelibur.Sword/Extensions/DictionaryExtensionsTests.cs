using System;
using System.Collections.Generic;
using Nelibur.Sword.DataStructures;
using Nelibur.Sword.Extensions;
using Xunit;

namespace UnitTests.Nelibur.Sword.Extensions
{
    public sealed class DictionaryExtensionsTests
    {
        [Fact]
        public void GetValue_Absent_Empty()
        {
            var dictiionary = new Dictionary<string, int> { { "A", 1 } };
            Option<int> item = dictiionary.GetValue("B");

            Assert.False(item.HasValue);
        }

        [Fact]
        public void GetValue_Exists_Value()
        {
            var dictiionary = new Dictionary<string, int> { { "A", 1 } };
            Option<int> item = dictiionary.GetValue("A");

            Assert.True(item.HasValue);
            Assert.Equal(1, item.Value);
        }
    }
}
