using System;
using System.ServiceModel.Channels;
using Nelibur.ServiceModel.Services.Maps;

namespace Nelibur.ServiceModel.Services.Processors
{
    public sealed class RestServiceProcessor : NeliburService
    {
        private RestServiceProcessor()
        {
        }

        public static IConfiguration Configure(Action<IConfiguration> action)
        {
            action(_configuration);
            return _configuration;
        }

        public static void Process(Message message)
        {
            RequestMetadata metadata = _requests.FromRestMessage(message);
            ProcessOneWay(metadata);
        }

        public static Message ProcessWithResponse(Message message)
        {
            RequestMetadata metadata = _requests.FromRestMessage(message);
            return Process(metadata);
        }
    }
}
