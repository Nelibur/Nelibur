using System;

namespace Nelibur.Sword.DataStructures
{
    public sealed class Option<TValue>
    {
        private static readonly Option<TValue> _empty = new Option<TValue>(default(TValue), false);
        private readonly bool _hasValue;

        public Option(TValue value, bool hasValue = true)
        {
            _hasValue = hasValue;
            Value = value;
        }

        public static Option<TValue> Empty
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

        public TValue Value { get; private set; }
    }
}
