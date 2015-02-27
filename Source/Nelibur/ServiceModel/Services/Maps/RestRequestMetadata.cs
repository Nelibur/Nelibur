using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Xml;
using Nelibur.ServiceModel.Serializers;

namespace Nelibur.ServiceModel.Services.Maps
{
    internal sealed class RestRequestMetadata : RequestMetadata
    {
        private const string ContentTypeHeader = "Content-Type";
        private const string OctetStreamContentType = "application/octet-stream";
        private readonly object _request;
        private readonly WebOperationContext _webOperationContext;

        internal RestRequestMetadata(Message message, Type targetType) : base(targetType)
        {
            _webOperationContext = WebOperationContext.Current;
            OperationType = GetOperationType(message);
            _request = CreateRequest(message, targetType);
        }

        public override string OperationType { get; protected set; }

        public override Message CreateResponse(object response)
        {
            var stream = response as Stream;
            if (stream == null)
            {
                var serializer = new DataContractJsonSerializer(response.GetType());
                return _webOperationContext.CreateJsonResponse(response, serializer);
            }
            return _webOperationContext.CreateStreamResponse(stream, GetResponseContentType());
        }

        public override TRequest GetRequest<TRequest>()
        {
            return (TRequest)_request;
        }

        private static object CreateRequestFromContent(Message message, Type targetType)
        {
            using (var stream = new MemoryStream())
            {
                XmlDictionaryWriter writer = JsonReaderWriterFactory.CreateJsonWriter(stream);
                message.WriteMessage(writer);
                writer.Flush();
                var serializer = new DataContractJsonSerializer(targetType);
                stream.Position = 0;
                return serializer.ReadObject(stream);
            }
        }

        private static string GetOperationType(Message message)
        {
            var httpReq = (HttpRequestMessageProperty)message.Properties[HttpRequestMessageProperty.Name];
            return httpReq.Method;
        }

        private object CraeteRequestFromUrl(Type targetType)
        {
            UriTemplateMatch templateMatch = _webOperationContext.IncomingRequest.UriTemplateMatch;
            NameValueCollection queryParams = templateMatch.QueryParameters;
            return UrlSerializer.FromQueryParams(queryParams).GetRequestValue(targetType);
        }

        private object CreateRequest(Message message, Type targetType)
        {
            if (IsRequestByUrl())
            {
                return CraeteRequestFromUrl(targetType);
            }

            return CreateRequestFromContent(message, targetType);
        }

        private string GetResponseContentType()
        {
            string[] allKeys = _webOperationContext.OutgoingResponse.Headers.AllKeys;
            string contentTypeKey = allKeys.FirstOrDefault(x => string.Equals(x, ContentTypeHeader, StringComparison.OrdinalIgnoreCase));
            if (string.IsNullOrWhiteSpace(contentTypeKey))
            {
                return OctetStreamContentType;
            }
            return _webOperationContext.OutgoingResponse.Headers[contentTypeKey];
        }

        private bool IsRequestByUrl()
        {
            return OperationType == Operations.OperationType.Get ||
                   OperationType == Operations.OperationType.Delete;
        }
    }
}
