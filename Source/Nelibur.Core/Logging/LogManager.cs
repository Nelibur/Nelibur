using System;
using Nelibur.Core.Logging.InternalLoggers;

namespace Nelibur.Core.Logging
{
    public static class LogManager
    {
        private static readonly ILogFactory _logFactory = new NullLogFactory();

        public static ILog GetLogger(Type type)
        {
            return _logFactory.GetLogger(type);
        }
    }
}
