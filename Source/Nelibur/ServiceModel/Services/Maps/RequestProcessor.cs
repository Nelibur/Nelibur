using System;
using System.ServiceModel.Channels;
using Nelibur.Core;
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
                    throw Error.InvalidOperation(message);
            }
        }

        private void Delete(RequestMetadata metadata)
        {
            var service = (IDeleteOneWay<TRequest>)_creator();
            var request = metadata.GetRequest<TRequest>();
            service.DeleteOneWay(request);
        }

        private Message DeleteWithResponse(RequestMetadata metadata)
        {
            var service = (IDelete<TRequest>)_creator();
            var request = metadata.GetRequest<TRequest>();
            object result = service.Delete(request);
            return metadata.CreateResponse(result);
        }

        private void Get(RequestMetadata metadata)
        {
            var service = (IGetOneWay<TRequest>)_creator();
            var request = metadata.GetRequest<TRequest>();
            service.GetOneWay(request);
        }

        private Message GetWithResponse(RequestMetadata metadata)
        {
            var service = (IGet<TRequest>)_creator();
            var request = metadata.GetRequest<TRequest>();
            object result = service.Get(request);
            return metadata.CreateResponse(result);
        }

        private void Post(RequestMetadata metadata)
        {
            var service = (IPostOneWay<TRequest>)_creator();
            var request = metadata.GetRequest<TRequest>();
            service.PostOneWay(request);
        }

        private Message PostWithResponse(RequestMetadata metadata)
        {
            var service = (IPost<TRequest>)_creator();
            var request = metadata.GetRequest<TRequest>();
            object result = service.Post(request);
            return metadata.CreateResponse(result);
        }

        private void Put(RequestMetadata metadata)
        {
            var service = (IPutOneWay<TRequest>)_creator();
            var request = metadata.GetRequest<TRequest>();
            service.PutOneWay(request);
        }

        private Message PutWithResponse(RequestMetadata metadata)
        {
            var service = (IPut<TRequest>)_creator();
            var request = metadata.GetRequest<TRequest>();
            object result = service.Put(request);
            return metadata.CreateResponse(result);
        }
    }
}
