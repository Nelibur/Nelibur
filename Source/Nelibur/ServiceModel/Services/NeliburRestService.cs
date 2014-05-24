using System;
using System.ServiceModel.Channels;
using Nelibur.ServiceModel.Services.Maps;

namespace Nelibur.ServiceModel.Services
{
    public sealed class NeliburRestService : NeliburService
    {
        private NeliburRestService()
        {
        }

        public static IConfiguration Configure(Action<IConfiguration> action)
        {
            action(_configuration);
            return _configuration;
        }

        public static Message Process(Message message)
        {
            RequestMetadata metadata = _requests.FromRestMessage(message);
            return Process(metadata);
        }

        public static void ProcessOneWay(Message message)
        {
            RequestMetadata metadata = _requests.FromRestMessage(message);
            ProcessOneWay(metadata);
        }
    }
}
