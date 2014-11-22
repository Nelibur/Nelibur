using System;
using Moq;
using Nelibur.Sword.DataStructures;
using Nelibur.Sword.Extensions;
using Xunit;

namespace UnitTests.Nelibur.Sword.DataStructures
{
    public class OptionTests
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

        [Fact]
        public void MatchType_Empty_NotExecuted()
        {
            Option<Letter> option = Option<Letter>.Empty;

            var mock = new Mock<OptionTests>();

            option
                .MatchType<A>(x => mock.Object.OnMatchA(x));

            mock.Verify(x => x.OnMatchB(It.IsAny<B>()), Times.Never());
        }

        [Fact]
        public void MatchType_Matched_Executed()
        {
            Letter letter = new A();
            Option<Letter> option = letter.ToOption();

            var mock = new Mock<OptionTests>();

            option
                .MatchType<A>(x => mock.Object.OnMatchA(x));

            mock.Verify(x => x.OnMatchA(It.IsAny<A>()));
        }

        [Fact]
        public void MatchType_NotMatched_NotExecuted()
        {
            Letter letter = new A();
            Option<Letter> option = letter.ToOption();

            var mock = new Mock<OptionTests>();

            option
                .MatchType<B>(x => mock.Object.OnMatchB(x));

            mock.Verify(x => x.OnMatchB(It.IsAny<B>()), Times.Never());
        }

        [Fact]
        public void Match_Mathed_Executed()
        {
            var option = new Option<int>(1);
            var mock = new Mock<OptionTests>();

            option.Match(x => x == 1, mock.Object.OnMatch);

            mock.Verify(x => x.OnMatch(option.Value));
        }

        [Fact]
        public void Match_NotMathed_NotExecuted()
        {
            Option<int> option = Option<int>.Empty;
            var mock = new Mock<OptionTests>();

            option.Match(x => x == 1, mock.Object.OnMatch);

            mock.Verify(x => x.OnMatch(option.Value), Times.Never());
        }

        [Fact]
        public void ThrowOnEmpty_Empty_ThrowCustomException()
        {
            Assert.ThrowsDelegateWithReturn func = () =>
            {
                Option<int> empty = Option<int>.Empty;
                return empty.ThrowOnEmpty<NullReferenceException>();
            };
            Assert.Throws(typeof(NullReferenceException), func);
        }

        [Fact]
        public void ThrowOnEmpty_Empty_ThrowException()
        {
            Assert.ThrowsDelegateWithReturn func = () => Option<int>.Empty.ThrowOnEmpty(() => new NullReferenceException());
            Assert.Throws(typeof(NullReferenceException), func);
        }

        [Fact]
        public void ThrowOnEmpty_NotEmpty_NotThrowException()
        {
            Assert.DoesNotThrow(() => new Option<int>(1).ThrowOnEmpty(() => new NullReferenceException()));
        }

        protected virtual void OnMatch(int value)
        {
        }

        protected virtual void OnMatchA(A value)
        {
        }

        protected virtual void OnMatchB(B value)
        {
        }

        protected sealed class A : Letter
        {
        }

        protected sealed class B : Letter
        {
        }

        protected abstract class Letter
        {
        }
    }
}
