using System;
using Nelibur.Sword.DataStructures;

namespace Nelibur.Sword.Extensions
{
    public static class OptionExtensions
    {
        public static Option<TInput> Do<TInput>(this Option<TInput> value, Action<TInput> action)
        {
            if (value.HasValue)
            {
                action(value.Value);
            }
            return value;
        }

        public static Option<TInput> Do<TInput>(
            this Option<TInput> value, Func<TInput, bool> predicate, Action<TInput> action)
        {
            if (value.HasNoValue)
            {
                return value;
            }
            if (predicate(value.Value))
            {
                action(value.Value);
            }
            return value;
        }

        public static Option<TInput> DoOnEmpty<TInput>(this Option<TInput> value, Action action)
        {
            if (value.HasNoValue)
            {
                action();
            }
            return value;
        }

        public static Option<TInput> Finally<TInput>(this Option<TInput> value, Action<TInput> action)
        {
            action(value.Value);
            return value;
        }

        public static Option<TResult> Map<TInput, TResult>(this Option<TInput> value, Func<TInput, Option<TResult>> func)
        {
            if (value.HasNoValue)
            {
                return Option<TResult>.Empty;
            }
            return func(value.Value);
        }

        public static Option<TResult> Map<TInput, TResult>(this Option<TInput> value, Func<TInput, TResult> func)
        {
            if (value.HasNoValue)
            {
                return Option<TResult>.Empty;
            }
            return func(value.Value).ToOption();
        }

        public static Option<TResult> Map<TInput, TResult>(
            this Option<TInput> value, Func<TInput, bool> predicate, Func<TInput, TResult> func)
        {
            if (value.HasNoValue)
            {
                return Option<TResult>.Empty;
            }
            if (!predicate(value.Value))
            {
                return Option<TResult>.Empty;
            }
            return func(value.Value).ToOption();
        }

        public static Option<TResult> MapOnEmpty<TInput, TResult>(this Option<TInput> value, Func<TResult> func)
        {
            if (value.HasNoValue)
            {
                return func().ToOption();
            }
            return Option<TResult>.Empty;
        }

        public static Option<TValue> ThrowOnEmpty<TValue, TException>(this Option<TValue> value)
            where TException : Exception, new()
        {
            if (value.HasValue)
            {
                return value;
            }
            throw Error.Type<TException>();
        }

        public static Option<TValue> ThrowOnEmpty<TValue, TException>(this Option<TValue> value, Func<TException> func)
            where TException : Exception
        {
            if (value.HasValue)
            {
                return value;
            }
            throw func();
        }

        public static Option<TInput> Where<TInput>(this Option<TInput> value, Func<TInput, bool> predicate)
        {
            if (value.HasNoValue)
            {
                return Option<TInput>.Empty;
            }
            return predicate(value.Value) ? value : Option<TInput>.Empty;
        }
    }
}
