using System;
using System.Collections.Generic;

namespace Nelibur.Core
{
    public sealed class FuncComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> _comparer;

        public FuncComparer(Func<T, T, bool> comparer)
        {
            if (comparer == null)
            {
                throw Error.ArgumentNull("comparer");
            }
            _comparer = comparer;
        }

        public bool Equals(T x, T y)
        {
            return _comparer(x, y);
        }

        public int GetHashCode(T obj)
        {
            return obj.ToString().ToLower().GetHashCode();
        }
    }
}
