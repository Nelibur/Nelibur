using System;
using System.Runtime.Serialization;
using System.ServiceModel.Channels;
using System.Xml;
using Nelibur.ServiceModel.Contracts;
using Nelibur.ServiceModel.Services.Headers;

namespace Nelibur.ServiceModel.Services.Maps
{
    internal sealed class SoapRequestMetadata : RequestMetadata
    {
        private readonly MessageVersion _messageVersion;
        private readonly object _request;

        internal SoapRequestMetadata(Message message, Type targetType) : base(targetType)
        {
            _messageVersion = message.Version;
            _request = GetBody(message, targetType);
            OperationType = SoapOperationTypeHeader.ReadHeader(message);
        }

        public override string OperationType { get; protected set; }

        public override TRequest GetRequest<TRequest>()
        {
            return (TRequest)_request;
        }

        public override Message GetResponse(object response)
        {
            return Message.CreateMessage(_messageVersion, ServiceMetadata.Operations.ProcessResponse, response);
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
