using System;

namespace Nelibur.Core.Logging
{
    internal interface ILogFactory
    {
        ILog GetLogger(Type type);
    }
}
