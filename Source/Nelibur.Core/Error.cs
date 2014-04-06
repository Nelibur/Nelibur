using System;
using System.Configuration;

namespace Nelibur.Core
{
    public static class Error
    {
        public static Exception ArgumentNull(string paramName)
        {
            return new ArgumentNullException(paramName);
        }

        public static Exception ConfigurationError(string message)
        {
            return new ConfigurationErrorsException(message);
        }

        public static Exception InvalidOperation(string message)
        {
            return new InvalidOperationException(message);
        }

        public static Exception NotImplemented()
        {
            return new NotImplementedException();
        }

        public static Exception NotSupported()
        {
            return new NotSupportedException();
        }
    }
}
