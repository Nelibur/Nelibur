using System;
using System.Collections.Generic;
using Moq;
using Nelibur.Core.DataStructures;
using Nelibur.Core.Extensions;
using Xunit;
using Xunit.Extensions;

namespace UnitTests.Nelibur.Core.Extensions
{
    public sealed class BagExtensionsTest
    {
        public static IEnumerable<object[]> EmptyAndNotEmptyBags
        {
            get
            {
                yield return new object[] { new Bag<int>(1) };
                yield return new object[] { Bag<int>.Empty };
            }
        }

        [Fact]
        public void DoOnEmpty_EmptyBag_Executed()
        {
            var mock = new Mock<Action>();
            Bag<int>.Empty.DoOnEmpty(mock.Object);
            mock.Verify(x => x(), Times.Once);
        }

        [Fact]
        public void DoOnEmpty_NotEmptyBag_NotExecuted()
        {
            var mock = new Mock<Action>();
            new Bag<int>(1).DoOnEmpty(mock.Object);
            mock.Verify(x => x(), Times.Never);
        }

        [Fact]
        public void Do_EmptyBag_Executed()
        {
            var mock = new Mock<Action<int>>();
            Bag<int>.Empty.Do(mock.Object);
            mock.Verify(x => x(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void Do_NotEmptyBag_NotExecuted()
        {
            var mock = new Mock<Action<int>>();
            new Bag<int>(1).Do(mock.Object);
            mock.Verify(x => x(It.IsAny<int>()), Times.Once);
        }

        [Theory]
        [PropertyData("EmptyAndNotEmptyBags")]
        public void Finally_OnAnyBag_Executed(Bag<int> value)
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
            Bag<string> result = Bag<int>.Empty.Map(mock.Object);
            mock.Verify(x => x(It.IsAny<int>()), Times.Never);
            Assert.True(result.HasNoValue);
        }

        [Fact]
        public void Map_NotEmptyBag_Executed()
        {
            var mock = new Mock<Func<int, string>>();
            mock.Setup(x => x(1)).Returns("data");
            Bag<string> result = new Bag<int>(1).Map(mock.Object);
            mock.Verify(x => x(It.IsAny<int>()), Times.Once);
            Assert.True(result.HasValue);
        }

        [Fact]
        public void ThrowOnEmpty_EmptyBag_ThrowException()
        {
            Assert.ThrowsDelegateWithReturn func = () => Bag<int>.Empty.ThrowOnEmpty(() => new NullReferenceException());
            Assert.Throws(typeof(NullReferenceException), func);
        }

        [Fact]
        public void ThrowOnEmpty_NotEmptyBag_NotThrowException()
        {
            Assert.DoesNotThrow(() => new Bag<int>(1).ThrowOnEmpty(() => new NullReferenceException()));
        }
    }
}
