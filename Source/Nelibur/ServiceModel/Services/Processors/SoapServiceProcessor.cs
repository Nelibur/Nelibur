using System;
using System.ServiceModel.Channels;
using Nelibur.ServiceModel.Services.Maps;

namespace Nelibur.ServiceModel.Services.Processors
{
    public sealed class SoapServiceProcessor : NeliburService
    {
        private SoapServiceProcessor()
        {
        }

        public static IConfiguration Configure(Action<IConfiguration> action)
        {
            action(_configuration);
            return _configuration;
        }

        public static void Process(Message message)
        {
            RequestMetadata metadata = _requests.FromSoapMessage(message);
            ProcessOneWay(metadata);
        }

        public static Message ProcessWithResponse(Message message)
        {
            RequestMetadata metadata = _requests.FromSoapMessage(message);
            return Process(metadata);
        }
    }
}
