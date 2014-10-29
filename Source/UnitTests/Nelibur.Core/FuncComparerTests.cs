using System;
using Nelibur.Core;
using Xunit;

namespace UnitTests.Nelibur.Core
{
    public sealed class FuncComparerTests
    {
        [Fact]
        public void Equals_EqualValues_True()
        {
            var compare = new FuncComparer<A>((x, y) => x.Value == y.Value);
            var value = new A { Value = 1 };

            Assert.True(compare.Equals(value, value));
        }

        [Fact]
        public void Equals_NotEqualValues_False()
        {
            var compare = new FuncComparer<A>((x, y) => x.Value == y.Value);
            var value1 = new A { Value = 1 };
            var value2 = new A { Value = 2 };

            Assert.False(compare.Equals(value1, value2));
        }


        private class A
        {
            public int Value { get; set; }
        }
    }
}
