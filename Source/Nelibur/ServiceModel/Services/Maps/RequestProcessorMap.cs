using System;
using System.Collections.Generic;
using Nelibur.ServiceModel.Services.Operations;

namespace Nelibur.ServiceModel.Services.Maps
{
    internal sealed class RequestProcessorMap
    {
        private static readonly Dictionary<Type, IRequestProcessorContext> _repository =
            new Dictionary<Type, IRequestProcessorContext>();

        public void Add<TRequest, TProcessor>()
            where TRequest : class
            where TProcessor : IRequestOperation, new()
        {
            Type requestType = typeof(TRequest);
            IRequestProcessorContext context = new RequestProcessorContext<TRequest, TProcessor>(() => new TProcessor());
            _repository[requestType] = context;
        }

        public void Add<TRequest, TProcessor>(Func<TProcessor> creator)
            where TRequest : class
            where TProcessor : IRequestOperation
        {
            Type requestType = typeof(TRequest);
            IRequestProcessorContext context = new RequestProcessorContext<TRequest, TProcessor>(creator);
            _repository[requestType] = context;
        }

        public IRequestProcessorContext Get(Type requestType)
        {
            return _repository[requestType];
        }
    }
}
