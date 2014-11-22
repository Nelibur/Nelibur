using System;
using System.Collections.Generic;
using Moq;
using Nelibur.Sword.DataStructures;
using Nelibur.Sword.Extensions;
using Xunit;
using Xunit.Extensions;

namespace UnitTests.Nelibur.Sword.Extensions
{
    public sealed class OptionExtensionsTests
    {
        public static IEnumerable<object[]> EmptyAndNotEmptyOptions
        {
            get
            {
                yield return new object[] { new Option<int>(1) };
                yield return new object[] { Option<int>.Empty };
            }
        }

        [Fact]
        public void DoOnEmpty_Empty_Executed()
        {
            var mock = new Mock<Action>();
            Option<int>.Empty.DoOnEmpty(mock.Object);
            mock.Verify(x => x(), Times.Once);
        }

        [Fact]
        public void DoOnEmpty_NotEmpty_NotExecuted()
        {
            var mock = new Mock<Action>();
            new Option<int>(1).DoOnEmpty(mock.Object);
            mock.Verify(x => x(), Times.Never);
        }

        [Fact]
        public void Do_Empty_Executed()
        {
            var mock = new Mock<Action<int>>();
            Option<int>.Empty.Do(mock.Object);
            mock.Verify(x => x(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void Do_NotEmpty_NotExecuted()
        {
            var mock = new Mock<Action<int>>();
            new Option<int>(1).Do(mock.Object);
            mock.Verify(x => x(It.IsAny<int>()), Times.Once);
        }

        [Theory]
        [PropertyData("EmptyAndNotEmptyOptions")]
        public void Finally_OnAny_Executed(Option<int> value)
        {
            var mock = new Mock<Action<int>>();
            value.Finally(mock.Object);
            mock.Verify(x => x(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void Map_Empty_NotExecuted()
        {
            var mock = new Mock<Func<int, string>>();
            mock.Setup(x => x(1)).Returns("data");
            Option<string> result = Option<int>.Empty.Map(mock.Object);
            mock.Verify(x => x(It.IsAny<int>()), Times.Never);
            Assert.True(result.HasNoValue);
        }

        [Fact]
        public void Map_NotEmptyWithFalsePredicate_Executed()
        {
            var mock = new Mock<Func<int, string>>();
            mock.Setup(x => x(1)).Returns("data");
            Option<string> result = new Option<int>(1).Map(x => false, mock.Object);
            mock.Verify(x => x(It.IsAny<int>()), Times.Never);
            Assert.True(result.HasNoValue);
        }

        [Fact]
        public void Map_NotEmptyWithTruePredicate_Executed()
        {
            var mock = new Mock<Func<int, string>>();
            mock.Setup(x => x(1)).Returns("data");
            Option<string> result = new Option<int>(1).Map(x => true, mock.Object);
            mock.Verify(x => x(It.IsAny<int>()), Times.Once);
            Assert.True(result.HasValue);
        }

        [Fact]
        public void Map_NotEmpty_Executed()
        {
            var mock = new Mock<Func<int, string>>();
            mock.Setup(x => x(1)).Returns("data");
            Option<string> result = new Option<int>(1).Map(mock.Object);
            mock.Verify(x => x(It.IsAny<int>()), Times.Once);
            Assert.True(result.HasValue);
        }

        [Fact]
        public void Where_Empty_Filtered()
        {
            Option<Item> item = Option<Item>.Empty;
            Option<Item> result = item.Where(x => string.IsNullOrWhiteSpace(x.Data) == false);
            Assert.True(result.HasNoValue);
        }

        [Fact]
        public void Where_NotEmpty_Filtered()
        {
            Option<Item> item = new Item { Data = "Data" }.ToOption();
            Option<Item> result = item.Where(x => string.IsNullOrWhiteSpace(x.Data) == false);
            Assert.True(result.HasValue);
        }

        private sealed class Item
        {
            public string Data { get; set; }
        }
    }
}
