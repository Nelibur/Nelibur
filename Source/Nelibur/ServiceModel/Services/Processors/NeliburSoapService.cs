using System;
using System.ServiceModel.Channels;
using Nelibur.ServiceModel.Services.Maps;

namespace Nelibur.ServiceModel.Services.Processors
{
    public sealed class NeliburSoapService : NeliburService
    {
        private NeliburSoapService()
        {
        }

        public static IConfiguration Configure(Action<IConfiguration> action)
        {
            action(_configuration);
            return _configuration;
        }

        public static Message Process(Message message)
        {
            RequestMetadata metadata = _requests.FromSoapMessage(message);
            return Process(metadata);
        }

        public static void ProcessOneWay(Message message)
        {
            RequestMetadata metadata = _requests.FromSoapMessage(message);
            ProcessOneWay(metadata);
        }
    }
}
