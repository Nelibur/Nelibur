using System;
using System.Configuration;
using Nelibur.Core;
using Xunit;

namespace UnitTests.Nelibur.Core
{
    public sealed class ErrorTest
    {
        [Fact]
        public void ArgumentNull()
        {
            Assert.IsType<ArgumentNullException>(Error.ArgumentNull(string.Empty));
        }

        [Fact]
        public void ConfigurationError()
        {
            Assert.IsType<ConfigurationErrorsException>(Error.ConfigurationError(string.Empty));
        }

        [Fact]
        public void InvalidOperation()
        {
            Assert.IsType<InvalidOperationException>(Error.InvalidOperation(string.Empty));
        }

        [Fact]
        public void NotImplemented()
        {
            Assert.IsType<NotImplementedException>(Error.NotImplemented());
        }

        [Fact]
        public void NotSupported()
        {
            Assert.IsType<NotSupportedException>(Error.NotSupported());
        }
    }
}
