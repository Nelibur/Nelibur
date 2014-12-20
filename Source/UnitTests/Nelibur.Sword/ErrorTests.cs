﻿using System;
using System.Configuration;
using Nelibur.Sword.Core;
using Xunit;

namespace UnitTests.Nelibur.Sword
{
    public sealed class ErrorTests
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
        public void ErrorByType()
        {
            Assert.IsType<NotImplementedException>(Error.Type<NotImplementedException>());
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
