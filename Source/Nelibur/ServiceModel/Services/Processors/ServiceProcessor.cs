using System;
using System.ServiceModel.Channels;

using Nelibur.ServiceModel.Services.Maps;
using Nelibur.ServiceModel.Services.Operations;

namespace Nelibur.ServiceModel.Services.Processors
{
    public abstract class ServiceProcessor
    {
        internal static readonly RequestMetadataMap _requests = new RequestMetadataMap();
        protected static readonly Configuration _configuration = new Configuration();
        private static readonly RequestProcessorMap _requestProcessors = new RequestProcessorMap();

        protected static void Process(RequestMetadata requestMetaData)
        {
            IRequestProcessor processor = _requestProcessors.Get(requestMetaData.Type);
            processor.Process(requestMetaData);
        }

        protected static Message ProcessWithResponse(RequestMetadata requestMetaData)
        {
            IRequestProcessor processor = _requestProcessors.Get(requestMetaData.Type);
            return processor.ProcessWithResponse(requestMetaData);
        }

        protected sealed class Configuration : IConfiguration
        {
            public void Bind<TRequest, TProcessor>(Func<TProcessor> creator)
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

            public void Bind<TRequest, TProcessor>()
                where TRequest : class
                where TProcessor : IRequestOperation, new()
            {
                Bind<TRequest, TProcessor>(() => new TProcessor());
            }
        }
    }
}
