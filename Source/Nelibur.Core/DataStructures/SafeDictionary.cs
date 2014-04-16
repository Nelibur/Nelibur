using System;
using System.Collections.Generic;

namespace Nelibur.Core.DataStructures
{
    internal sealed class SafeDictionary<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> _dictionary = new Dictionary<TKey, TValue>();
        private readonly object _locker = new object();

        public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
        {
            TValue result;
            lock (_locker)
            {
                if (_dictionary.TryGetValue(key, out result))
                {
                    return result;
                }
            }
            TValue newValue = valueFactory(key);
            lock (_locker)
            {
                if (_dictionary.TryGetValue(key, out result))
                {
                    return result;
                }
                _dictionary[key] = newValue;
            }
            return newValue;
        }
    }
}
