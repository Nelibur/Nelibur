using System;
using System.ServiceModel.Channels;
using Nelibur.ServiceModel.Contracts;
using Nelibur.ServiceModel.Services.Operations;

namespace Nelibur.ServiceModel.Services.Maps
{
    internal sealed class RequestProcessorContext<TRequest, TProcessor> : IRequestProcessorContext
        where TRequest : class
        where TProcessor : IRequestOperation
    {
        private readonly Func<TProcessor> _creator;

        public RequestProcessorContext(Func<TProcessor> creator)
        {
            _creator = creator;
        }

        public Message Process(RequestMetadata metadata)
        {
            switch (metadata.OperationType)
            {
                case OperationType.Get:
                    return Get(metadata);
                case OperationType.Post:
                    return Post(metadata);
                case OperationType.Put:
                    return Put(metadata);
                case OperationType.Delete:
                    return Delete(metadata);
                default:
                    string message = string.Format("Invalid operation type: {0}", metadata.OperationType);
                    throw new InvalidOperationException(message);
            }
        }

        public void ProcessOneWay(RequestMetadata metadata)
        {
            switch (metadata.OperationType)
            {
                case OperationType.Post:
                    PostOneWay(metadata);
                    break;
                case OperationType.Put:
                    PutOneWay(metadata);
                    break;
                case OperationType.Delete:
                    DeleteOneWay(metadata);
                    break;
                default:
                    string message = string.Format("Invalid operation type: {0}", metadata.OperationType);
                    throw new InvalidOperationException(message);
            }
        }

        private Message Delete(RequestMetadata metadata)
        {
            var service = (IDelete<TRequest>)_creator();
            var request = metadata.GetRequest<TRequest>();
            object result = service.Delete(request);
            return Message.CreateMessage(metadata.MessageVersion, ServiceMetadata.Operations.ProcessResponse, result);
        }

        private void DeleteOneWay(RequestMetadata metadata)
        {
            var service = (IDeleteOneWay<TRequest>)_creator();
            var request = metadata.GetRequest<TRequest>();
            service.DeleteOneWay(request);
        }

        private Message Get(RequestMetadata metadata)
        {
            var service = (IGet<TRequest>)_creator();
            var request = metadata.GetRequest<TRequest>();
            object result = service.Get(request);
            return Message.CreateMessage(metadata.MessageVersion, ServiceMetadata.Operations.ProcessResponse, result);
        }

        private Message Post(RequestMetadata metadata)
        {
            var service = (IPost<TRequest>)_creator();
            var request = metadata.GetRequest<TRequest>();
            object result = service.Post(request);
            return Message.CreateMessage(metadata.MessageVersion, ServiceMetadata.Operations.ProcessResponse, result);
        }

        private void PostOneWay(RequestMetadata metadata)
        {
            var service = (IPostOneWay<TRequest>)_creator();
            var request = metadata.GetRequest<TRequest>();
            service.PostOneWay(request);
        }

        private Message Put(RequestMetadata metadata)
        {
            var service = (IPut<TRequest>)_creator();
            var request = metadata.GetRequest<TRequest>();
            object result = service.Put(request);
            return Message.CreateMessage(metadata.MessageVersion, ServiceMetadata.Operations.ProcessResponse, result);
        }

        private void PutOneWay(RequestMetadata metadata)
        {
            var service = (IPutOneWay<TRequest>)_creator();
            var request = metadata.GetRequest<TRequest>();
            service.PutOneWay(request);
        }
    }
}
