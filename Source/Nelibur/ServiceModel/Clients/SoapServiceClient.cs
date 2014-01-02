using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using Nelibur.ServiceModel.Contracts;
using Nelibur.ServiceModel.Services.Headers;

namespace Nelibur.ServiceModel.Clients
{
    public class SoapServiceClient
    {
        private const string EndpointConfigurationName = "NeliburSoapService";

        public static void Delete<TRequest>(TRequest request)
            where TRequest : class
        {
            Process(request, OperationTypeHeader.Delete);
        }

        public static Task DeleteAsync<TRequest>(TRequest request)
            where TRequest : class
        {
            return Task.Run(() => Delete(request));
        }

        public static TResponse Get<TRequest, TResponse>(TRequest request)
            where TRequest : class
            where TResponse : class
        {
            return ProcessWithResponse<TRequest, TResponse>(request, OperationTypeHeader.Get);
        }

        public static Task<TResponse> GetAsync<TRequest, TResponse>(TRequest request)
            where TRequest : class
            where TResponse : class
        {
            return Task.Run(() => Get<TRequest, TResponse>(request));
        }

        public static void Post<TRequest>(TRequest request)
            where TRequest : class
        {
            Process(request, OperationTypeHeader.Post);
        }

        public static TResponse Post<TRequest, TResponse>(TRequest request)
            where TRequest : class
            where TResponse : class
        {
            return ProcessWithResponse<TRequest, TResponse>(request, OperationTypeHeader.Post);
        }

        public static Task<TResponse> PostAsync<TRequest, TResponse>(TRequest request)
            where TRequest : class
            where TResponse : class
        {
            return Task.Run(() => Post<TRequest, TResponse>(request));
        }

        public static Task PostAsync<TRequest>(TRequest request)
            where TRequest : class
        {
            return Task.Run(() => Post(request));
        }

        public static void Put<TRequest>(TRequest request)
            where TRequest : class
        {
            Process(request, OperationTypeHeader.Put);
        }

        public static TResponse Put<TRequest, TResponse>(TRequest request)
            where TRequest : class
            where TResponse : class
        {
            return ProcessWithResponse<TRequest, TResponse>(request, OperationTypeHeader.Put);
        }

        public static Task PutAsync<TRequest>(TRequest request)
            where TRequest : class
        {
            return Task.Run(() => Put(request));
        }

        public static Task<TResponse> PutAsync<TRequest, TResponse>(TRequest request)
            where TRequest : class
            where TResponse : class
        {
            return Task.Run(() => Put<TRequest, TResponse>(request));
        }

        private static Message CreateMessage<TRequest>(
            TRequest request, MessageHeader actionHeader, MessageVersion messageVersion)
        {
            Message message = Message.CreateMessage(
                messageVersion, ServiceMetadata.Operations.Process, request);
            var contentTypeHeader = new ContentTypeHeader(typeof(TRequest));
            message.Headers.Add(contentTypeHeader);
            message.Headers.Add(actionHeader);
            return message;
        }

        private static Message CreateMessageWithResponse<TRequest>(
            TRequest request, MessageHeader actionHeader, MessageVersion messageVersion)
        {
            Message message = Message.CreateMessage(
                messageVersion, ServiceMetadata.Operations.ProcessWithResponse, request);
            var contentTypeHeader = new ContentTypeHeader(typeof(TRequest));
            message.Headers.Add(contentTypeHeader);
            message.Headers.Add(actionHeader);
            return message;
        }

        private static void Process<TRequest>(TRequest request, MessageHeader operationType)
            where TRequest : class
        {
            using (var factory = new ChannelFactory<ISoapService>(EndpointConfigurationName))
            {
                MessageVersion messageVersion = factory.Endpoint.Binding.MessageVersion;
                Message message = CreateMessage(request, operationType, messageVersion);
                ISoapService channel = factory.CreateChannel();
                channel.Process(message);
            }
        }

        private static TResponse ProcessWithResponse<TRequest, TResponse>(TRequest request, MessageHeader operationType)
            where TRequest : class
            where TResponse : class
        {
            using (var factory = new ChannelFactory<ISoapService>(EndpointConfigurationName))
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
