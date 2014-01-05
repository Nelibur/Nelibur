using System;
using System.Runtime.Serialization;
using System.ServiceModel.Channels;
using System.Xml;
using Nelibur.ServiceModel.Contracts;
using Nelibur.ServiceModel.Services.Headers;

namespace Nelibur.ServiceModel.Services.Maps
{
    internal sealed class RequestMetadata : IRequestMetadata
    {
        private readonly MessageVersion _messageVersion;
        private readonly object _request;

        private RequestMetadata(Message message, Type targetType)
        {
            _messageVersion = message.Version;
            Type = targetType;
            _request = GetBody(message, targetType);
            OperationType = OperationTypeHeader.ReadHeader(message);
        }

        public string OperationType { get; private set; }

        public Type Type { get; private set; }

        public TRequest GetRequest<TRequest>()
        {
            return (TRequest)_request;
        }

        public Message GetResponse(object response)
        {
            return Message.CreateMessage(_messageVersion, ServiceMetadata.Operations.ProcessResponse, response);
        }

        internal static IRequestMetadata FromMessage(Message message, Type targetType)
        {
            return new RequestMetadata(message, targetType);
        }

        private static object GetBody(Message message, Type targetType)
        {
            using (XmlDictionaryReader reader = message.GetReaderAtBodyContents())
            {
                var serializer = new DataContractSerializer(targetType);
                return serializer.ReadObject(reader);
            }
        }
    }
}
