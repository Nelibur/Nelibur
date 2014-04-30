using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using Nelibur.Core;
using Nelibur.ServiceModel.Contracts;
using Nelibur.ServiceModel.Services.Headers;

namespace Nelibur.ServiceModel.Clients
{
    public sealed class SoapServiceClient
    {
        private readonly string _endpointConfigurationName;

        /// <summary>
        ///     Create new instance of <see cref="SoapServiceClient" /> .
        /// </summary>
        /// <param name="endpointConfigurationName">WCF's endpoint name.</param>
        public SoapServiceClient(string endpointConfigurationName)
        {
            if (string.IsNullOrWhiteSpace(endpointConfigurationName))
            {
                throw Error.ConfigurationError("Invalid endpointConfigurationName: Is null or empty");
            }
            _endpointConfigurationName = endpointConfigurationName;
        }

        public void Delete<TRequest>(TRequest request)
            where TRequest : class
        {
            Process(request, SoapOperationTypeHeader.Delete);
        }

        public TResponse Delete<TRequest, TResponse>(TRequest request)
            where TRequest : class
        {
            return ProcessWithResponse<TRequest, TResponse>(request, SoapOperationTypeHeader.Delete);
        }

        public Task DeleteAsync<TRequest>(TRequest request)
            where TRequest : class
        {
            return Task.Run(() => Process(request, SoapOperationTypeHeader.Delete));
        }

        public Task<TResponse> DeleteAsync<TRequest, TResponse>(TRequest request)
            where TRequest : class
        {
            return Task.Run(() => ProcessWithResponse<TRequest, TResponse>(request, SoapOperationTypeHeader.Delete));
        }

        public void Get<TRequest>(TRequest request)
            where TRequest : class
        {
            Process(request, SoapOperationTypeHeader.Get);
        }

        public TResponse Get<TRequest, TResponse>(TRequest request)
            where TRequest : class
        {
            return ProcessWithResponse<TRequest, TResponse>(request, SoapOperationTypeHeader.Get);
        }

        public Task GetAsync<TRequest>(TRequest request)
            where TRequest : class
        {
            return Task.Run(() => Process(request, SoapOperationTypeHeader.Get));
        }

        public Task<TResponse> GetAsync<TRequest, TResponse>(TRequest request)
            where TRequest : class
        {
            return Task.Run(() => ProcessWithResponse<TRequest, TResponse>(request, SoapOperationTypeHeader.Get));
        }

        public void Post<TRequest>(TRequest request)
            where TRequest : class
        {
            Process(request, SoapOperationTypeHeader.Post);
        }

        public TResponse Post<TRequest, TResponse>(TRequest request)
            where TRequest : class
        {
            return ProcessWithResponse<TRequest, TResponse>(request, SoapOperationTypeHeader.Post);
        }

        public Task<TResponse> PostAsync<TRequest, TResponse>(TRequest request)
            where TRequest : class
        {
            return Task.Run(() => ProcessWithResponse<TRequest, TResponse>(request, SoapOperationTypeHeader.Post));
        }

        public Task PostAsync<TRequest>(TRequest request)
            where TRequest : class
        {
            return Task.Run(() => Process(request, SoapOperationTypeHeader.Post));
        }

        public void Put<TRequest>(TRequest request)
            where TRequest : class
        {
            Process(request, SoapOperationTypeHeader.Put);
        }

        public TResponse Put<TRequest, TResponse>(TRequest request)
            where TRequest : class
        {
            return ProcessWithResponse<TRequest, TResponse>(request, SoapOperationTypeHeader.Put);
        }

        public Task PutAsync<TRequest>(TRequest request)
            where TRequest : class
        {
            return Task.Run(() => Put(request));
        }

        public Task<TResponse> PutAsync<TRequest, TResponse>(TRequest request)
            where TRequest : class
        {
            return Task.Run(() => ProcessWithResponse<TRequest, TResponse>(request, SoapOperationTypeHeader.Put));
        }

        private static Message CreateMessage<TRequest>(
            TRequest request, MessageHeader actionHeader, MessageVersion messageVersion)
        {
            Message message = Message.CreateMessage(messageVersion, SoapServiceMetadata.Action.Process, request);
            var contentTypeHeader = new SoapContentTypeHeader(typeof(TRequest));
            message.Headers.Add(contentTypeHeader);
            message.Headers.Add(actionHeader);
            return message;
        }

        private static Message CreateMessageWithResponse<TRequest>(
            TRequest request, MessageHeader actionHeader, MessageVersion messageVersion)
        {
            Message message = Message.CreateMessage(messageVersion, SoapServiceMetadata.Action.ProcessWithResponse, request);
            var contentTypeHeader = new SoapContentTypeHeader(typeof(TRequest));
            message.Headers.Add(contentTypeHeader);
            message.Headers.Add(actionHeader);
            return message;
        }

        private void Process<TRequest>(TRequest request, MessageHeader operationType)
            where TRequest : class
        {
            using (var factory = new ChannelFactory<ISoapService>(_endpointConfigurationName))
            {
                MessageVersion messageVersion = factory.Endpoint.Binding.MessageVersion;
                Message message = CreateMessage(request, operationType, messageVersion);
                ISoapService channel = factory.CreateChannel();
                channel.Process(message);
            }
        }

        private TResponse ProcessWithResponse<TRequest, TResponse>(TRequest request, MessageHeader operationType)
            where TRequest : class
        {
            using (var factory = new ChannelFactory<ISoapService>(_endpointConfigurationName))
            {
                MessageVersion messageVersion = factory.Endpoint.Binding.MessageVersion;
                Message message = CreateMessageWithResponse(request, operationType, messageVersion);
                ISoapService channel = factory.CreateChannel();
                Message result = channel.ProcessWithResponse(message);
                return result.GetBody<TResponse>();
            }
        }
    }
}
