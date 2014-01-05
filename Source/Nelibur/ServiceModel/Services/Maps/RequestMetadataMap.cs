using System;
using System.Collections.Generic;
using System.ServiceModel.Channels;
using Nelibur.ServiceModel.Services.Headers;

namespace Nelibur.ServiceModel.Services.Maps
{
    internal sealed class RequestMetadataMap
    {
        private readonly Dictionary<string, Type> _requestTypes =
            new Dictionary<string, Type>();

        internal void Add<TRequest>()
            where TRequest : class
        {
            Type requestType = typeof(TRequest);
            _requestTypes[requestType.Name] = requestType;
        }

        internal RequestMetadata FromMessage(Message message)
        {
            string typeName = SoapContentTypeHeader.ReadHeader(message);
            Type targetType = _requestTypes[typeName];
            return RequestMetadata.FromSoapMessage(message, targetType);
        }

        internal RequestMetadata FromRestMessage(Message message)
        {
            string typeName = RestContentTypeHeader.ReadHeader(message);
            Type targetType = _requestTypes[typeName];
            return RequestMetadata.FromRestMessage(message, targetType);
        }
    }
}
