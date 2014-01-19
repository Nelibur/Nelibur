using System;
using Nelibur.ServiceModel.Services.Maps;
using Nelibur.ServiceModel.Services.Operations;

namespace Nelibur.ServiceModel.Services.Processors
{
    public abstract class ServiceProcessor
    {
        internal static readonly RequestProcessorMap _requestProcessors = new RequestProcessorMap();
        internal static readonly RequestMetadataMap _requests = new RequestMetadataMap();

        public ServiceProcessor Bind<TRequest, TProcessor>()
            where TRequest : class
            where TProcessor : IRequestOperation, new()
        {
            _requestProcessors.Add<TRequest, TProcessor>();
            _requests.Add<TRequest>();
            return this;
        }

        public ServiceProcessor Bind<TRequest, TProcessor>(Func<TProcessor> creator)
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
