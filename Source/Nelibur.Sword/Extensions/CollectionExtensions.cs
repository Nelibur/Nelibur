using System;
using System.Collections.Generic;

namespace Nelibur.Sword.Extensions
{
    public static class CollectionExtensions
    {
        public static bool IsEmpty<T>(this ICollection<T> value)
        {
            return value.Count == 0;
        }

        public static bool IsNotEmpty<T>(this ICollection<T> value)
        {
            return !IsEmpty(value);
        }

        public static bool IsNullOrEmpty<T>(this ICollection<T> value)
        {
            return value.IsNull() || value.Count == 0;
        }
    }
}
