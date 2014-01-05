using System;
using System.ServiceModel.Channels;
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

        public void Process(IRequestMetadata metadata)
        {
            switch (metadata.OperationType)
            {
                case OperationType.Post:
                    Post(metadata);
                    break;
                case OperationType.Put:
                    Put(metadata);
                    break;
                case OperationType.Delete:
                    Delete(metadata);
                    break;
                default:
                    string message = string.Format("Invalid operation type: {0}", metadata.OperationType);
                    throw new InvalidOperationException(message);
            }
        }

        public Message ProcessWithResponse(IRequestMetadata metadata)
        {
            switch (metadata.OperationType)
            {
                case OperationType.Get:
                    return GetWithResponse(metadata);
                case OperationType.Post:
                    return PostWithResponse(metadata);
                case OperationType.Put:
                    return PutWithResponse(metadata);
                case OperationType.Delete:
                    return DeleteWithResponse(metadata);
                default:
                    string message = string.Format("Invalid operation type: {0}", metadata.OperationType);
                    throw new InvalidOperationException(message);
            }
        }

        private void Delete(IRequestMetadata metadata)
        {
            var service = (IDelete<TRequest>)_creator();
            var request = metadata.GetRequest<TRequest>();
            service.Delete(request);
        }

        private Message DeleteWithResponse(IRequestMetadata metadata)
        {
            var service = (IDeleteWithResponse<TRequest>)_creator();
            var request = metadata.GetRequest<TRequest>();
            object result = service.DeleteWithResponse(request);
            return metadata.GetResponse(result);
        }

        private Message GetWithResponse(IRequestMetadata metadata)
        {
            var service = (IGetWithResponse<TRequest>)_creator();
            var request = metadata.GetRequest<TRequest>();
            object result = service.GetWithResponse(request);
            return metadata.GetResponse(result);
        }

        private void Post(IRequestMetadata metadata)
        {
            var service = (IPost<TRequest>)_creator();
            var request = metadata.GetRequest<TRequest>();
            service.Post(request);
        }

        private Message PostWithResponse(IRequestMetadata metadata)
        {
            var service = (IPostWithResponse<TRequest>)_creator();
            var request = metadata.GetRequest<TRequest>();
            object result = service.PostWithResponse(request);
            return metadata.GetResponse(result);
        }

        private void Put(IRequestMetadata metadata)
        {
            var service = (IPut<TRequest>)_creator();
            var request = metadata.GetRequest<TRequest>();
            service.Put(request);
        }

        private Message PutWithResponse(IRequestMetadata metadata)
        {
            var service = (IPutWithResponse<TRequest>)_creator();
            var request = metadata.GetRequest<TRequest>();
            object result = service.PutWithResponse(request);
            return metadata.GetResponse(result);
        }
    }
}
