using System;
using System.ServiceModel.Channels;
using Nelibur.ServiceModel.Services.Maps;
using Nelibur.ServiceModel.Services.Operations;

namespace Nelibur.ServiceModel.Services
{
    public sealed class RestServcieProcessor
    {
        private static readonly RequestProcessorMap _requestProcessors = new RequestProcessorMap();
        private static readonly RequestMetadataMap _requests = new RequestMetadataMap();

        public static void Process(Message message)
        {
            IRequestMetadata requestMetaData = _requests.FromRestMessage(message);
            IRequestProcessorContext context = _requestProcessors.Get(requestMetaData.Type);
            context.Process(requestMetaData);
        }

        public static Message ProcessWithResponse(Message message)
        {
            IRequestMetadata requestMetaData = _requests.FromRestMessage(message);
            IRequestProcessorContext context = _requestProcessors.Get(requestMetaData.Type);
            return context.ProcessWithResponse(requestMetaData);
        }

        public RestServcieProcessor Bind<TRequest, TProcessor>()
            where TRequest : class
            where TProcessor : IRequestOperation, new()
        {
            _requestProcessors.Add<TRequest, TProcessor>();
            _requests.Add<TRequest>();
            return this;
        }

        public RestServcieProcessor Bind<TRequest, TProcessor>(Func<TProcessor> creator)
            where TRequest : class
            where TProcessor : IRequestOperation
        {
            if (creator == null)
            {
                throw new ArgumentNullException("creator");
            }
            _requestProcessors.Add<TRequest, TProcessor>(creator);
            _requests.Add<TRequest>();
            return this;
        }
    }
}
