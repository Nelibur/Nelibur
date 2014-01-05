using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Text;
using System.Xml;
using Nelibur.ServiceModel.Services.Headers;

namespace Nelibur.ServiceModel.Services.Maps
{
    internal sealed class RestRequestMetadata : IRequestMetadata
    {
        private readonly object _request;

        private RestRequestMetadata(Message message, Type targetType)
        {
            OperationType = GetOperationType(message);
            _request = GetBody(message, targetType);
            Type = targetType;
        }

        public string OperationType { get; private set; }

        public Type Type { get; private set; }

        public TRequest GetRequest<TRequest>()
        {
            return (TRequest)_request;
        }

        public Message GetResponse(object response)
        {
            var serializer = new DataContractJsonSerializer(response.GetType());
            return WebOperationContext.Current.CreateJsonResponse(response, serializer);
        }

        internal static IRequestMetadata FromMessage(Message message, Type targetType)
        {
            return new RestRequestMetadata(message, targetType);
        }

        private static string GetOperationType(Message message)
        {
            var httpReq = (HttpRequestMessageProperty)message.Properties[HttpRequestMessageProperty.Name];
            return httpReq.Method;
        }

        private object GetBody(Message message, Type targetType)
        {
            if (OperationType == Operations.OperationType.Get)
            {
                string content = RestContentDataHeader.ReadHeader(message);
                byte[] bytes = Encoding.UTF8.GetBytes(content);

                using (var stream = new MemoryStream(bytes))
                {
                    var serializer = new DataContractJsonSerializer(targetType);
                    stream.Position = 0;
                    return serializer.ReadObject(stream);
                }
            }

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
    }
}
