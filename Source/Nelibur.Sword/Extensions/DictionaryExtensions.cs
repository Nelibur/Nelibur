using System;
using System.Collections.Generic;
using Nelibur.Sword.DataStructures;

namespace Nelibur.Sword.Extensions
{
    public static class DictionaryExtensions
    {
        public static Option<TValue> GetValue<TKey, TValue>(this IDictionary<TKey, TValue> value, TKey key)
        {
            TValue result;
            bool exists = value.TryGetValue(key, out result);
            return new Option<TValue>(result, exists);
        }
    }
}
