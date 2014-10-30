using System;

namespace Nelibur.Sword.Logging
{
    internal interface ILogFactory
    {
        ILog GetLogger(Type type);
    }
}
