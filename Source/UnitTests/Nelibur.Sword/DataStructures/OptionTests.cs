using Nelibur.Sword.DataStructures;
using Xunit;

namespace UnitTests.Nelibur.Sword.DataStructures
{
    public sealed class OptionTests
    {
        [Fact]
        public void Default_ValueAbsent_True()
        {
            const int Value = 1;
            var bag = new Option<int>(Value, false);
            Assert.False(bag.HasValue);
            Assert.True(bag.HasNoValue);
            Assert.Equal(Value, bag.Value);
        }

        [Fact]
        public void Default_ValueExist_True()
        {
            const int Value = 1;
            var bag = new Option<int>(Value);
            Assert.True(bag.HasValue);
            Assert.False(bag.HasNoValue);
            Assert.Equal(Value, bag.Value);
        }

        [Fact]
        public void Empty_ReferenceType_Null()
        {
            Option<string> option = Option<string>.Empty;
            Assert.Equal(null, option.Value);
        }

        [Fact]
        public void Empty_ValueType_Zero()
        {
            Option<int> option = Option<int>.Empty;
            Assert.Equal(0, option.Value);
        }
    }
}
