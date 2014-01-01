using System;

namespace Nelibur.Core.Logging.InternalLoggers
{
    internal sealed class NullLogFactory : ILogFactory
    {
        public ILog GetLogger(Type type)
        {
            return new NullLogger();
        }
    }
}
