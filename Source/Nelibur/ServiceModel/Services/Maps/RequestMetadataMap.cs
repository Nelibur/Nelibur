using System;
using System.Collections.Generic;
using System.ServiceModel.Channels;
using Nelibur.ServiceModel.Services.Headers;

namespace Nelibur.ServiceModel.Services.Maps
{
    internal sealed class RequestMetadataMap
    {
        private static readonly Dictionary<string, Type> _requestTypes =
            new Dictionary<string, Type>();

        public void Add<TRequest>()
            where TRequest : class
        {
            Type requestType = typeof(TRequest);
            _requestTypes[requestType.Name] = requestType;
        }

        public RequestMetadata FromMessage(Message message)
        {
            string typeName = ContentTypeHeader.ReadHeader(message);
            Type targetType = _requestTypes[typeName];
            return RequestMetadata.FromMessage(message, targetType);
        }
    }
}
