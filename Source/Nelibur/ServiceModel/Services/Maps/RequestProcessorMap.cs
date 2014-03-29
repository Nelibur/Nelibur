using System;
using System.Collections.Generic;

using Nelibur.ServiceModel.Services.Operations;

namespace Nelibur.ServiceModel.Services.Maps
{
    internal sealed class RequestProcessorMap
    {
        private readonly Dictionary<Type, IRequestProcessor> _repository =
            new Dictionary<Type, IRequestProcessor>();

        public void Add<TRequest, TProcessor>(Func<TProcessor> creator)
            where TRequest : class
            where TProcessor : IRequestOperation
        {
            Type requestType = typeof(TRequest);
            IRequestProcessor context = new RequestProcessor<TRequest, TProcessor>(creator);
            _repository[requestType] = context;
        }

        public IRequestProcessor Get(Type requestType)
        {
            return _repository[requestType];
        }
    }
}
