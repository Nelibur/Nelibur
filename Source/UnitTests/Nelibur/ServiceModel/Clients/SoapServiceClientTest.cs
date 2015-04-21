using System;
using System.Configuration;
using Nelibur.ServiceModel.Clients;
using Xunit;
using Xunit.Extensions;

namespace UnitTests.Nelibur.ServiceModel.Clients
{
    public sealed class SoapServiceClientTest
    {
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void SoapServiceClient_InvalidEndpoint_ThrowException(string value)
        {
            Assert.Throws<ConfigurationErrorsException>(() => new SoapServiceClient(value));
        }
    }
}
