using System;
using System.Collections.Generic;
using Moq;
using Nelibur.Sword.DataStructures;
using Nelibur.Sword.Extensions;
using Xunit;
using Xunit.Extensions;

namespace UnitTests.Nelibur.Sword.Extensions
{
    public sealed class BagExtensionsTests
    {
        public static IEnumerable<object[]> EmptyAndNotEmptyBags
        {
            get
            {
                yield return new object[] { new Option<int>(1) };
                yield return new object[] { Option<int>.Empty };
            }
        }

        [Fact]
        public void DoOnEmpty_EmptyBag_Executed()
        {
            var mock = new Mock<Action>();
            Option<int>.Empty.DoOnEmpty(mock.Object);
            mock.Verify(x => x(), Times.Once);
        }

        [Fact]
        public void DoOnEmpty_NotEmptyBag_NotExecuted()
        {
            var mock = new Mock<Action>();
            new Option<int>(1).DoOnEmpty(mock.Object);
            mock.Verify(x => x(), Times.Never);
        }

        [Fact]
        public void Do_EmptyBag_Executed()
        {
            var mock = new Mock<Action<int>>();
            Option<int>.Empty.Do(mock.Object);
            mock.Verify(x => x(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void Do_NotEmptyBag_NotExecuted()
        {
            var mock = new Mock<Action<int>>();
            new Option<int>(1).Do(mock.Object);
            mock.Verify(x => x(It.IsAny<int>()), Times.Once);
        }

        [Theory]
        [PropertyData("EmptyAndNotEmptyBags")]
        public void Finally_OnAnyBag_Executed(Option<int> value)
        {
            var mock = new Mock<Action<int>>();
            value.Finally(mock.Object);
            mock.Verify(x => x(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void Map_EmptyBag_NotExecuted()
        {
            var mock = new Mock<Func<int, string>>();
            mock.Setup(x => x(1)).Returns("data");
            Option<string> result = Option<int>.Empty.Map(mock.Object);
            mock.Verify(x => x(It.IsAny<int>()), Times.Never);
            Assert.True(result.HasNoValue);
        }

        [Fact]
        public void Map_NotEmptyBagWithFalsePredicate_Executed()
        {
            var mock = new Mock<Func<int, string>>();
            mock.Setup(x => x(1)).Returns("data");
            Option<string> result = new Option<int>(1).Map(x => false, mock.Object);
            mock.Verify(x => x(It.IsAny<int>()), Times.Never);
            Assert.True(result.HasNoValue);
        }

        [Fact]
        public void Map_NotEmptyBagWithTruePredicate_Executed()
        {
            var mock = new Mock<Func<int, string>>();
            mock.Setup(x => x(1)).Returns("data");
            Option<string> result = new Option<int>(1).Map(x => true, mock.Object);
            mock.Verify(x => x(It.IsAny<int>()), Times.Once);
            Assert.True(result.HasValue);
        }

        [Fact]
        public void Map_NotEmptyBag_Executed()
        {
            var mock = new Mock<Func<int, string>>();
            mock.Setup(x => x(1)).Returns("data");
            Option<string> result = new Option<int>(1).Map(mock.Object);
            mock.Verify(x => x(It.IsAny<int>()), Times.Once);
            Assert.True(result.HasValue);
        }

        [Fact]
        public void ThrowOnEmpty_EmptyBag_ThrowCustomException()
        {
            Assert.ThrowsDelegateWithReturn func = () =>
            {
                Option<int> empty = Option<int>.Empty;
                return empty.ThrowOnEmpty<int, NullReferenceException>();
            };
            Assert.Throws(typeof(NullReferenceException), func);
        }

        [Fact]
        public void ThrowOnEmpty_EmptyBag_ThrowException()
        {
            Assert.ThrowsDelegateWithReturn func = () => Option<int>.Empty.ThrowOnEmpty(() => new NullReferenceException());
            Assert.Throws(typeof(NullReferenceException), func);
        }

        [Fact]
        public void ThrowOnEmpty_NotEmptyBag_NotThrowException()
        {
            Assert.DoesNotThrow(() => new Option<int>(1).ThrowOnEmpty(() => new NullReferenceException()));
        }

        [Fact]
        public void Where_EmptyBag_Filtered()
        {
            Option<Item> item = Option<Item>.Empty;
            Option<Item> result = item.Where(x => string.IsNullOrWhiteSpace(x.Data) == false);
            Assert.True(result.HasNoValue);
        }

        [Fact]
        public void Where_NotEmptyBag_Filtered()
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
