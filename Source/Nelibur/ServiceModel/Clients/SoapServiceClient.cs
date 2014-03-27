using System;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

using Nelibur.ServiceModel.Contracts;
using Nelibur.ServiceModel.Services.Headers;

namespace Nelibur.ServiceModel.Clients
{
    public sealed class SoapServiceClient : ServiceClient
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
                throw new ConfigurationErrorsException("Invalid endpointConfigurationName: Is null or empty");
            }
            _endpointConfigurationName = endpointConfigurationName;
        }

        protected override Task DeleteAsyncCore<TRequest>(TRequest request)
        {
            return Task.Run(() => DeleteCore(request));
        }

        protected override Task<TResponse> DeleteAsyncCore<TRequest, TResponse>(TRequest request)
        {
            return Task.Run(() => ProcessWithResponse<TRequest, TResponse>(request, SoapOperationTypeHeader.Delete));
        }

        protected override void DeleteCore<TRequest>(TRequest request)
        {
            Process(request, SoapOperationTypeHeader.Delete);
        }

        protected override TResponse DeleteCore<TRequest, TResponse>(TRequest request)
        {
            return ProcessWithResponse<TRequest, TResponse>(request, SoapOperationTypeHeader.Delete);
        }

        protected override Task GetAsyncCore<TRequest>(TRequest request)
        {
            return Task.Run(() => GetCore(request));
        }

        protected override Task<TResponse> GetAsyncCore<TRequest, TResponse>(TRequest request)
        {
            return Task.Run(() => GetCore<TRequest, TResponse>(request));
        }

        protected override TResponse GetCore<TRequest, TResponse>(TRequest request)
        {
            return ProcessWithResponse<TRequest, TResponse>(request, SoapOperationTypeHeader.Get);
        }

        protected override void GetCore<TRequest>(TRequest request)
        {
            Process(request, SoapOperationTypeHeader.Get);
        }

        protected override Task<TResponse> PostAsyncCore<TRequest, TResponse>(TRequest request)
        {
            return Task.Run(() => ProcessWithResponse<TRequest, TResponse>(request, SoapOperationTypeHeader.Post));
        }

        protected override Task PostAsyncCore<TRequest>(TRequest request)
        {
            return Task.Run(() => PostCore(request));
        }

        protected override TResponse PostCore<TRequest, TResponse>(TRequest request)
        {
            return ProcessWithResponse<TRequest, TResponse>(request, SoapOperationTypeHeader.Post);
        }

        protected override void PostCore<TRequest>(TRequest request)
        {
            Process(request, SoapOperationTypeHeader.Post);
        }

        protected override Task PutAsyncCore<TRequest>(TRequest request)
        {
            return Task.Run(() => PutCore(request));
        }

        protected override Task<TResponse> PutAsyncCore<TRequest, TResponse>(TRequest request)
        {
            return Task.Run(() => ProcessWithResponse<TRequest, TResponse>(request, SoapOperationTypeHeader.Put));
        }

        protected override TResponse PutCore<TRequest, TResponse>(TRequest request)
        {
            return ProcessWithResponse<TRequest, TResponse>(request, SoapOperationTypeHeader.Put);
        }

        protected override void PutCore<TRequest>(TRequest request)
        {
            Process(request, SoapOperationTypeHeader.Put);
        }

        private static Message CreateMessage<TRequest>(
            TRequest request, MessageHeader actionHeader, MessageVersion messageVersion)
        {
            Message message = Message.CreateMessage(
                messageVersion, SoapServiceMetadata.Action.Process, request);
            var contentTypeHeader = new SoapContentTypeHeader(typeof(TRequest));
            message.Headers.Add(contentTypeHeader);
            message.Headers.Add(actionHeader);
            return message;
        }

        private static Message CreateMessageWithResponse<TRequest>(
            TRequest request, MessageHeader actionHeader, MessageVersion messageVersion)
        {
            Message message = Message.CreateMessage(
                messageVersion,
                SoapServiceMetadata.Action.ProcessWithResponse,
                request);
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
