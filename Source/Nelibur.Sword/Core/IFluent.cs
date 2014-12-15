using System;
using System.ComponentModel;

namespace Nelibur.Sword.Core
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IFluent
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        bool Equals(object other);

        [EditorBrowsable(EditorBrowsableState.Never)]
        int GetHashCode();

        [EditorBrowsable(EditorBrowsableState.Never)]
        Type GetType();

        [EditorBrowsable(EditorBrowsableState.Never)]
        string ToString();
    }
}
