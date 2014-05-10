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

        public void Delete(object request)
        {
            Process(request, SoapOperationTypeHeader.Delete);
        }

        public TResponse Delete<TResponse>(object request)
        {
            return ProcessWithResponse<TResponse>(request, SoapOperationTypeHeader.Delete);
        }

        public Task DeleteAsync(object request)
        {
            return Task.Run(() => Process(request, SoapOperationTypeHeader.Delete));
        }

        public Task<TResponse> DeleteAsync<TResponse>(object request)
        {
            return Task.Run(() => ProcessWithResponse<TResponse>(request, SoapOperationTypeHeader.Delete));
        }

        public void Get(object request)
        {
            Process(request, SoapOperationTypeHeader.Get);
        }

        public TResponse Get<TResponse>(object request)
        {
            return ProcessWithResponse<TResponse>(request, SoapOperationTypeHeader.Get);
        }

        public Task GetAsync(object request)
        {
            return Task.Run(() => Process(request, SoapOperationTypeHeader.Get));
        }

        public Task<TResponse> GetAsync<TResponse>(object request)
        {
            return Task.Run(() => ProcessWithResponse<TResponse>(request, SoapOperationTypeHeader.Get));
        }

        public void Post(object request)
        {
            Process(request, SoapOperationTypeHeader.Post);
        }

        public TResponse Post<TResponse>(object request)
        {
            return ProcessWithResponse<TResponse>(request, SoapOperationTypeHeader.Post);
        }

        public Task<TResponse> PostAsync<TResponse>(object request)
        {
            return Task.Run(() => ProcessWithResponse<TResponse>(request, SoapOperationTypeHeader.Post));
        }

        public Task PostAsync(object request)
        {
            return Task.Run(() => Process(request, SoapOperationTypeHeader.Post));
        }

        public void Put(object request)
        {
            Process(request, SoapOperationTypeHeader.Put);
        }

        public TResponse Put<TResponse>(object request)
        {
            return ProcessWithResponse<TResponse>(request, SoapOperationTypeHeader.Put);
        }

        public Task PutAsync(object request)
        {
            return Task.Run(() => Put(request));
        }

        public Task<TResponse> PutAsync<TResponse>(object request)
        {
            return Task.Run(() => ProcessWithResponse<TResponse>(request, SoapOperationTypeHeader.Put));
        }

        private static Message CreateMessage(
            object request, MessageHeader actionHeader, MessageVersion messageVersion)
        {
            Message message = Message.CreateMessage(messageVersion, SoapServiceMetadata.Action.Process, request);
            var contentTypeHeader = new SoapContentTypeHeader(request.GetType());
            message.Headers.Add(contentTypeHeader);
            message.Headers.Add(actionHeader);
            return message;
        }

        private static Message CreateMessageWithResponse(
            object request, MessageHeader actionHeader, MessageVersion messageVersion)
        {
            Message message = Message.CreateMessage(messageVersion, SoapServiceMetadata.Action.ProcessWithResponse, request);
            var contentTypeHeader = new SoapContentTypeHeader(request.GetType());
            message.Headers.Add(contentTypeHeader);
            message.Headers.Add(actionHeader);
            return message;
        }

        private void Process(object request, MessageHeader operationType)
        {
            using (var factory = new ChannelFactory<ISoapService>(_endpointConfigurationName))
            {
                MessageVersion messageVersion = factory.Endpoint.Binding.MessageVersion;
                Message message = CreateMessage(request, operationType, messageVersion);
                ISoapService channel = factory.CreateChannel();
                channel.Process(message);
            }
        }

        private TResponse ProcessWithResponse<TResponse>(object request, MessageHeader operationType)
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
