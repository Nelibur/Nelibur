using System;
using System.Runtime.Serialization;
using System.ServiceModel.Channels;
using System.Xml;
using Nelibur.ServiceModel.Services.Headers;

namespace Nelibur.ServiceModel.Services.Maps
{
    internal sealed class RequestMetadata
    {
        private readonly Message _message;
        private readonly object _request;

        private RequestMetadata(Message message, object request, Type type, string operationType)
        {
            _message = message;
            _request = request;
            Type = type;
            OperationType = operationType;
        }

        public MessageVersion MessageVersion
        {
            get { return _message.Version; }
        }

        public string OperationType { get; private set; }

        public Type Type { get; private set; }

        public static RequestMetadata FromMessage(Message message, Type targetType)
        {
            object request = GetBody(message, targetType);
            string operationType = OperationTypeHeader.ReadHeader(message);
            return new RequestMetadata(message, request, targetType, operationType);
        }

        public TRequest GetRequest<TRequest>()
        {
            return (TRequest)_request;
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
