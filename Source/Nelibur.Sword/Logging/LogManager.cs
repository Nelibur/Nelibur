using System;
using Nelibur.Sword.Logging.InternalLoggers;

namespace Nelibur.Sword.Logging
{
    internal static class LogManager
    {
        private static readonly ILogFactory _logFactory = new NullLogFactory();

        public static ILog GetLogger(Type type)
        {
            return _logFactory.GetLogger(type);
        }
    }
}
