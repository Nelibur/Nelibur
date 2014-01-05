using System;
using System.ServiceModel.Channels;

namespace Nelibur.ServiceModel.Services.Maps
{
    internal abstract class RequestMetadata : IRequestMetadata
    {
        protected RequestMetadata(Type targetType)
        {
            Type = targetType;
        }

        public abstract string OperationType { get; protected set; }

        public Type Type { get; private set; }

        public abstract TRequest GetRequest<TRequest>();

        public abstract Message GetResponse(object response);

        internal static IRequestMetadata FromRestMessage(Message message, Type targetType)
        {
            return new RestRequestMetadata(message, targetType);
        }

        internal static IRequestMetadata FromSoapMessage(Message message, Type targetType)
        {
            return new SoapRequestMetadata(message, targetType);
        }
    }
}
