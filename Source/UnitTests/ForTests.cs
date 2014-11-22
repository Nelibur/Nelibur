using System;
using Nelibur.Sword.DataStructures;
using Nelibur.Sword.Extensions;
using Xunit;

namespace UnitTests
{
    public sealed class ForTests
    {
        [Fact]
        public void Test()
        {
            var option = new Option<int>(1);
            option
                .Match(x => x == 1, x => Console.WriteLine("Bob"))
                .Match(x => x == 1, x => Console.WriteLine("Lo"))
                .Match(x => x == 2, x => Console.WriteLine("Tito"));

            Letter letter = new A();
            Option<Letter> letterOption = letter.ToOption();
            letterOption
                .Match<A>(x => Console.WriteLine("A"))
                .Match<B>(x => Console.WriteLine("B"));
        }

        private sealed class A : Letter
        {
        }

        private sealed class B : Letter
        {
        }

        private abstract class Letter
        {
        }
    }

    public static class MatchExtensions
    {
        public static Option<T> Match<T>(this Option<T> value, Func<T, bool> predicate, Action<T> action)
        {
            if (value.HasNoValue)
            {
                return Option<T>.Empty;
            }
            if (predicate(value.Value) == false)
            {
                return value;
            }
            action(value.Value);
            return value;
        }
    }
}
