using System;
using System.Collections.Generic;

namespace Nelibur.Core.Extensions
{
    public static class IntExtensions
    {
        public static void Times(this int value, Action action)
        {
            for (int i = 0; i < value; i++)
            {
                action();
            }
        }

        public static void Times(this int value, Action<int> action)
        {
            for (int i = 0; i < value; i++)
            {
                action(i);
            }
        }

        public static IEnumerable<int> Times(this int value)
        {
            for (int i = 0; i < value; i++)
            {
                yield return i;
            }
        }
    }
}
