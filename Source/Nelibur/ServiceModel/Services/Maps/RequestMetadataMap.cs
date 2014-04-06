using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using Nelibur.Core;
using Nelibur.ServiceModel.Serializers;
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

        internal RequestMetadata FromRestMessage(Message message)
        {
            UriTemplateMatch templateMatch = WebOperationContext.Current.IncomingRequest.UriTemplateMatch;
            NameValueCollection queryParams = templateMatch.QueryParameters;
            string typeName = UrlSerializer.FromQueryParams(queryParams).GetTypeValue();
            Type targetType = GetRequestType(typeName);
            return RequestMetadata.FromRestMessage(message, targetType);
        }

        internal RequestMetadata FromSoapMessage(Message message)
        {
            string typeName = SoapContentTypeHeader.ReadHeader(message);
            Type targetType = GetRequestType(typeName);
            return RequestMetadata.FromSoapMessage(message, targetType);
        }

        private Type GetRequestType(string typeName)
        {
            Type result;
            if (_requestTypes.TryGetValue(typeName, out result))
            {
                return result;
            }
            string errorMessage = string.Format(
                "Binding on {0} is absent. Use the Bind method on an appropriate ServiceProcessor", typeName);
            throw Error.InvalidOperation(errorMessage);
        }
    }
}
