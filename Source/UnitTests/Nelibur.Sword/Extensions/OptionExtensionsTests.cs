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
            Option<int> result = Option<int>.Empty.DoOnEmpty(mock.Object);
            mock.Verify(x => x(), Times.Once);

            Assert.True(result.HasNoValue);
            Assert.Equal(default(int), result.Value);
        }

        [Fact]
        public void DoOnEmpty_NotEmpty_NotExecuted()
        {
            var mock = new Mock<Action>();
            Option<int> result = new Option<int>(1).DoOnEmpty(mock.Object);

            mock.Verify(x => x(), Times.Never);
            Assert.Equal(1, result.Value);
        }

        [Fact]
        public void Do_Empty_Executed()
        {
            var mock = new Mock<Action<int>>();
            Option<int> result = Option<int>.Empty.Do(mock.Object);

            mock.Verify(x => x(It.IsAny<int>()), Times.Never);
            Assert.Equal(default(int), result.Value);
        }

        [Fact]
        public void Do_TruePredicate_Executed()
        {
            var mock = new Mock<Action<int>>();
            var result = new Option<int>(5).Do(x => true, mock.Object);

            mock.Verify(x => x(It.IsAny<int>()), Times.Once);
            Assert.NotEqual(default(int), result.Value);
        }

        [Fact]
        public void Do_FalsePredicate_NotExecuted()
        {
            var mock = new Mock<Action<int>>();
            Option<int> result = Option<int>.Empty.Do(x => false, mock.Object);

            mock.Verify(x => x(It.IsAny<int>()), Times.Never);
            Assert.Equal(default(int), result.Value);
        }

        [Fact]
        public void Do_NotEmpty_NotExecuted()
        {
            var mock = new Mock<Action<int>>();
            Option<int> result = new Option<int>(1).Do(mock.Object);

            mock.Verify(x => x(It.IsAny<int>()), Times.Once);
            Assert.Equal(1, result.Value);
        }

        [Theory]
        [PropertyData("EmptyAndNotEmptyOptions")]
        public void Finally_OnAny_Executed(Option<int> value)
        {
            var mock = new Mock<Action<int>>();
            Option<int> result = value.Finally(mock.Object);

            mock.Verify(x => x(It.IsAny<int>()), Times.Once);
            Assert.Equal(value.Value, result.Value);
        }

        [Fact]
        public void MapOnEmpty_NotEmpty_Executed()
        {
            var mock = new Mock<Func<int>>();
            mock.Setup(x => x()).Returns(2);
            Option<int> result = Option<int>.Empty.MapOnEmpty(mock.Object);

            mock.Verify(x => x(), Times.Once);
            Assert.Equal(2, result.Value);
        }

        [Fact]
        public void MapOnEmpty_NotEmpty_NotExecuted()
        {
            var mock = new Mock<Func<int>>();
            mock.Setup(x => x()).Returns(2);
            Option<int> result = new Option<int>(1).MapOnEmpty(mock.Object);

            mock.Verify(x => x(), Times.Never);
            Assert.Equal(1, result.Value);
        }

        [Fact]
        public void MapWithOption_Empty_NotExecuted()
        {
            var mock = new Mock<Func<int, Option<string>>>();
            mock.Setup(x => x(1)).Returns(new Option<string>("data"));

            Option<string> result = Option<int>.Empty.Map(mock.Object);

            mock.Verify(x => x(It.IsAny<int>()), Times.Never);
            Assert.True(result.HasNoValue);
            Assert.Equal(default(string), result.Value);
        }

        [Fact]
        public void MapWithOption_NotEmpty_Executed()
        {
            var mock = new Mock<Func<int, Option<string>>>();
            mock.Setup(x => x(1)).Returns(new Option<string>("data"));

            Option<string> result = new Option<int>(1).Map(mock.Object);

            mock.Verify(x => x(It.IsAny<int>()), Times.Once);
            Assert.True(result.HasValue);
            Assert.Equal("data", result.Value);
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
        public void SelectMany_Empty_Ok()
        {
            Option<int> result = from x in 5.ToOption()
                from y in Option<int>.Empty
                select x + y;
            Assert.True(result.HasNoValue);
        }

        [Fact]
        public void SelectMany_NotEmpty_Ok()
        {
            Option<int> result = from x in 5.ToOption()
                from y in 2.ToOption()
                from z in 3.ToOption()
                select x + y + z;
            Assert.True(result.HasValue);
            Assert.Equal(10, result.Value);
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
