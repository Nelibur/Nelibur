using System;
using System.Configuration;
using Nelibur.Sword.Reflection;

namespace Nelibur.Sword.Core
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

        public static Exception Type<TException>()
            where TException : Exception, new()
        {
            return DelegateFactory.CreateCtor<TException>()();
        }
    }
}
