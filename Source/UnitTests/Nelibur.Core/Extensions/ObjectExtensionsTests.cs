using Nelibur.Core.DataStructures;
using Nelibur.Core.Extensions;
using Xunit;

namespace UnitTests.Nelibur.Core.Extensions
{
    public sealed class ObjectExtensionsTests
    {
        [Fact]
        public void IsNotNull_NotNull_True()
        {
            Assert.True(new object().IsNotNull());
        }

        [Fact]
        public void IsNotNull_Null_False()
        {
            object value = null;
            Assert.False(value.IsNotNull());
        }

        [Fact]
        public void IsNull_NotNull_False()
        {
            Assert.False(new object().IsNull());
        }

        [Fact]
        public void IsNull_Null_True()
        {
            Assert.True(ObjectExtensions.IsNull(null));
        }

        [Fact]
        public void ToBag_Null_Empty()
        {
            Assert.False(((object)(null)).ToBag().HasValue);
        }

        [Fact]
        public void ToBag_Null_NotEmpty()
        {
            Assert.True(new object().ToBag().HasValue);
        }

        [Fact]
        public void ToType_InvalidType_HasNoValue()
        {
            object value = 1;
            Bag<bool> result = value.ToType<bool>();
            Assert.True(result.HasNoValue);
        }

        [Fact]
        public void ToType_ValidType_HasValue()
        {
            object value = true;
            Bag<bool> result = value.ToType<bool>();
            Assert.True(result.HasValue);
        }
    }
}
