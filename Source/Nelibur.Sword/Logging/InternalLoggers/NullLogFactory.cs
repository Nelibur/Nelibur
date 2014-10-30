using System;

namespace Nelibur.Sword.Logging.InternalLoggers
{
    internal sealed class NullLogFactory : ILogFactory
    {
        public ILog GetLogger(Type type)
        {
            return new NullLogger();
        }
    }
}
