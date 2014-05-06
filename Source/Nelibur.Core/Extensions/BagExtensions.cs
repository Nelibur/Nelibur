using System;
using Nelibur.Core.DataStructures;

namespace Nelibur.Core.Extensions
{
    public static class BagExtensions
    {
        public static Bag<TInput> Do<TInput>(this Bag<TInput> value, Action<TInput> action)
        {
            if (value.HasValue)
            {
                action(value.Value);
            }
            return value;
        }

        public static Bag<TInput> Do<TInput>(
            this Bag<TInput> value, Func<TInput, bool> predicate, Action<TInput> action)
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

        public static Bag<TInput> DoOnEmpty<TInput>(this Bag<TInput> value, Action action)
        {
            if (value.HasNoValue)
            {
                action();
            }
            return value;
        }

        public static Bag<TInput> Finally<TInput>(this Bag<TInput> value, Action<TInput> action)
        {
            action(value.Value);
            return value;
        }

        public static Bag<TResult> Map<TInput, TResult>(this Bag<TInput> value, Func<TInput, Bag<TResult>> func)
        {
            if (value.HasNoValue)
            {
                return Bag<TResult>.Empty;
            }
            return func(value.Value);
        }

        public static Bag<TResult> Map<TInput, TResult>(this Bag<TInput> value, Func<TInput, TResult> func)
        {
            if (value.HasNoValue)
            {
                return Bag<TResult>.Empty;
            }
            return func(value.Value).ToBag();
        }

        public static Bag<TResult> Map<TInput, TResult>(
            this Bag<TInput> value, Func<TInput, bool> predicate, Func<TInput, TResult> func)
        {
            if (value.HasNoValue)
            {
                return Bag<TResult>.Empty;
            }
            if (!predicate(value.Value))
            {
                return Bag<TResult>.Empty;
            }
            return func(value.Value).ToBag();
        }

        public static Bag<TResult> MapOnEmpty<TInput, TResult>(this Bag<TInput> value, Func<TResult> func)
        {
            if (value.HasNoValue)
            {
                return func().ToBag();
            }
            return Bag<TResult>.Empty;
        }

        public static Bag<TValue> ThrowOnEmpty<TValue, TException>(this Bag<TValue> value)
            where TException : Exception, new()
        {
            if (value.HasValue)
            {
                return value;
            }
            throw Error.Type<TException>();
        }

        public static Bag<TValue> ThrowOnEmpty<TValue, TException>(this Bag<TValue> value, Func<TException> func)
            where TException : Exception
        {
            if (value.HasValue)
            {
                return value;
            }
            throw func();
        }

        public static Bag<TInput> Where<TInput>(this Bag<TInput> value, Func<TInput, bool> predicate)
        {
            if (value.HasNoValue)
            {
                return Bag<TInput>.Empty;
            }
            return predicate(value.Value) ? value : Bag<TInput>.Empty;
        }
    }
}
