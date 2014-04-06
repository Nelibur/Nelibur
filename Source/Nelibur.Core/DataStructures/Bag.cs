using System;

namespace Nelibur.Core.DataStructures
{
    public sealed class Bag<TValue>
    {
        private static readonly Bag<TValue> _empty = new Bag<TValue>(default(TValue), false);
        private readonly bool _hasValue;

        public Bag(TValue value, bool hasValue = true)
        {
            _hasValue = hasValue;
            Value = value;
        }

        public static Bag<TValue> Empty
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
