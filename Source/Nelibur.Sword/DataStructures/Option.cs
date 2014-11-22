using System;

namespace Nelibur.Sword.DataStructures
{
    public sealed class Option<T>
    {
        private static readonly Option<T> _empty = new Option<T>(default(T), false);
        private readonly bool _hasValue;

        public Option(T value, bool hasValue = true)
        {
            _hasValue = hasValue;
            Value = value;
        }

        public static Option<T> Empty
        {
            get { return _empty; }
        }

        public bool HasNoValue
        {
            get { return !_hasValue; }
        }

        public bool HasValue
        {
            get { return _hasValue; }
        }

        public T Value { get; private set; }

        public Option<T> Match<TTarget>(Action<TTarget> action)
            where TTarget : T
        {
            if (HasNoValue)
            {
                return Empty;
            }
            if (Value.GetType() == typeof(TTarget))
            {
                action((TTarget)Value);
            }
            return this;
        }
    }
}
