using System;
using System.ServiceModel.Channels;
using Nelibur.ServiceModel.Services.Maps;
using Nelibur.ServiceModel.Services.Operations;

namespace Nelibur.ServiceModel.Services
{
    public static class SoapServiceProcessor
    {
        private static readonly RequestProcessorMap _requestProcessors = new RequestProcessorMap();
        private static readonly RequestMetadataMap _requests = new RequestMetadataMap();

        public static void Bind<TRequest, TProcessor>()
            where TRequest : class
            where TProcessor : IRequestOperation, new()
        {
            _requestProcessors.Add<TRequest, TProcessor>();
            _requests.Add<TRequest>();
        }

        public static void Bind<TRequest, TProcessor>(Func<TProcessor> creator)
            where TRequest : class
            where TProcessor : IRequestOperation
        {
            if (creator == null)
            {
                throw new ArgumentNullException("creator");
            }
            _requestProcessors.Add<TRequest, TProcessor>(creator);
            _requests.Add<TRequest>();
        }

        public static Message Process(Message message)
        {
            RequestMetadata requestMetaData = _requests.FromMessage(message);
            IRequestProcessorContext context = _requestProcessors.Get(requestMetaData.Type);
            return context.Process(requestMetaData);
        }

        public static void ProcessWithoutResonse(Message message)
        {
            RequestMetadata requestMetaData = _requests.FromMessage(message);
            IRequestProcessorContext context = _requestProcessors.Get(requestMetaData.Type);
            context.ProcessOneWay(requestMetaData);
        }
    }
}
