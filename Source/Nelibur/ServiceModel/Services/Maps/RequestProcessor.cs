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
                    throw Error.InvalidOperation(message);
            }
        }

        public void ProcessOneWay(RequestMetadata metadata)
        {
            switch (metadata.OperationType)
            {
                case OperationType.Get:
                    GetOneWay(metadata);
                    break;
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
                    throw Error.InvalidOperation(message);
            }
        }

        private Message Delete(RequestMetadata metadata)
        {
            var service = (IDelete<TRequest>)_creator();
            var request = metadata.GetRequest<TRequest>();
            object result = service.Delete(request);
            return metadata.CreateResponse(result);
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
            return metadata.CreateResponse(result);
        }

        private void GetOneWay(RequestMetadata metadata)
        {
            var service = (IGetOneWay<TRequest>)_creator();
            var request = metadata.GetRequest<TRequest>();
            service.GetOneWay(request);
        }

        private Message Post(RequestMetadata metadata)
        {
            var service = (IPost<TRequest>)_creator();
            var request = metadata.GetRequest<TRequest>();
            object result = service.Post(request);
            return metadata.CreateResponse(result);
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
            return metadata.CreateResponse(result);
        }

        private void PutOneWay(RequestMetadata metadata)
        {
            var service = (IPutOneWay<TRequest>)_creator();
            var request = metadata.GetRequest<TRequest>();
            service.PutOneWay(request);
        }
    }
}
