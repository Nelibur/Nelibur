using Nelibur.Core.DataStructures;
using Xunit;

namespace UnitTests.Nelibur.Core.DataStructures
{
    public sealed class BagTests
    {
        [Fact]
        public void Default_ValueAbsent_True()
        {
            const int Value = 1;
            var bag = new Bag<int>(Value, false);
            Assert.False(bag.HasValue);
            Assert.True(bag.HasNoValue);
            Assert.Equal(Value, bag.Value);
        }

        [Fact]
        public void Default_ValueExist_True()
        {
            const int Value = 1;
            var bag = new Bag<int>(Value);
            Assert.True(bag.HasValue);
            Assert.False(bag.HasNoValue);
            Assert.Equal(Value, bag.Value);
        }

        [Fact]
        public void Empty_ReferenceType_Null()
        {
            Bag<string> bag = Bag<string>.Empty;
            Assert.Equal(null, bag.Value);
        }

        [Fact]
        public void Empty_ValueType_Zero()
        {
            Bag<int> bag = Bag<int>.Empty;
            Assert.Equal(0, bag.Value);
        }
    }
}
