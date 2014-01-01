using System.ComponentModel;
using Nelibur.Core.DataStructures;
using Nelibur.Core.Extensions;
using Xunit;

namespace UnitTests.Nelibur.Core.Extensions
{
    public sealed class EnumExtensionsTests
    {
        private enum Test
        {
            [Description("Test 1")]
            Test1,
            Test2
        }

        [Fact]
        public void GetAttribute_WithAttribute_Success()
        {
            Bag<DescriptionAttribute> bag = Test.Test1.GetAttribute<DescriptionAttribute>();
            Assert.True(bag.HasValue);
        }

        [Fact]
        public void GetAttribute_WithoutAttribute_Success()
        {
            Bag<DescriptionAttribute> bag = Test.Test2.GetAttribute<DescriptionAttribute>();
            Assert.False(bag.HasValue);
        }

        [Fact]
        public void ToDescription_WithTestEnum_Success()
        {
            Assert.Equal("Test 1", Test.Test1.ToDescription());
            Assert.Equal("Test2", Test.Test2.ToDescription());
        }
    }
}
