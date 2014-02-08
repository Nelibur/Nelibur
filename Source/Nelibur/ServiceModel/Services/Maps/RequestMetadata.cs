using System;
using System.ServiceModel.Channels;

namespace Nelibur.ServiceModel.Services.Maps
{
    public abstract class RequestMetadata
    {
        protected RequestMetadata(Type targetType)
        {
            Type = targetType;
        }

        public abstract string OperationType { get; protected set; }

        public Type Type { get; private set; }

        public static RequestMetadata FromRestMessage(Message message, Type targetType)
        {
            return new RestRequestMetadata(message, targetType);
        }

        public static RequestMetadata FromSoapMessage(Message message, Type targetType)
        {
            return new SoapRequestMetadata(message, targetType);
        }

        public abstract Message CreateResponse(object response);
        public abstract TRequest GetRequest<TRequest>();
    }
}
