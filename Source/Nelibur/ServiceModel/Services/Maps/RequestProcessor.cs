using System;
using System.ServiceModel.Channels;

using Nelibur.ServiceModel.Services.Operations;

namespace Nelibur.ServiceModel.Services.Maps
{
    internal sealed class RequestProcessor<TRequest, TProcessor> : IRequestProcessor
        where TRequest : class
        where TProcessor : IRequestOperation
    {
        private readonly Func<TProcessor> _creator;

        public RequestProcessor(Func<TProcessor> creator)
        {
            _creator = creator;
        }

        public void Process(RequestMetadata metadata)
        {
            switch (metadata.OperationType)
            {
                case OperationType.Get:
                    Get(metadata);
                    break;
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

        public Message ProcessWithResponse(RequestMetadata metadata)
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

        private void Delete(RequestMetadata metadata)
        {
            var service = (IDelete<TRequest>)_creator();
            var request = metadata.GetRequest<TRequest>();
            service.Delete(request);
        }

        private Message DeleteWithResponse(RequestMetadata metadata)
        {
            var service = (IDeleteWithResponse<TRequest>)_creator();
            var request = metadata.GetRequest<TRequest>();
            object result = service.DeleteWithResponse(request);
            return metadata.CreateResponse(result);
        }

        private void Get(RequestMetadata metadata)
        {
            var service = (IGet<TRequest>)_creator();
            var request = metadata.GetRequest<TRequest>();
            service.Get(request);
        }

        private Message GetWithResponse(RequestMetadata metadata)
        {
            var service = (IGetWithResponse<TRequest>)_creator();
            var request = metadata.GetRequest<TRequest>();
            object result = service.GetWithResponse(request);
            return metadata.CreateResponse(result);
        }

        private void Post(RequestMetadata metadata)
        {
            var service = (IPost<TRequest>)_creator();
            var request = metadata.GetRequest<TRequest>();
            service.Post(request);
        }

        private Message PostWithResponse(RequestMetadata metadata)
        {
            var service = (IPostWithResponse<TRequest>)_creator();
            var request = metadata.GetRequest<TRequest>();
            object result = service.PostWithResponse(request);
            return metadata.CreateResponse(result);
        }

        private void Put(RequestMetadata metadata)
        {
            var service = (IPut<TRequest>)_creator();
            var request = metadata.GetRequest<TRequest>();
            service.Put(request);
        }

        private Message PutWithResponse(RequestMetadata metadata)
        {
            var service = (IPutWithResponse<TRequest>)_creator();
            var request = metadata.GetRequest<TRequest>();
            object result = service.PutWithResponse(request);
            return metadata.CreateResponse(result);
        }
    }
}
