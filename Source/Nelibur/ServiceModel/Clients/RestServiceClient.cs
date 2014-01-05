using System.Configuration;
using System.IO;
using System.Runtime.Serialization.Json;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Threading.Tasks;
using System.Xml;
using Nelibur.ServiceModel.Contracts;
using Nelibur.ServiceModel.Services.Headers;
using Nelibur.ServiceModel.Services.Operations;

namespace Nelibur.ServiceModel.Clients
{
    public class RestServiceClient
    {
        private readonly string _endpointConfigurationName;

        /// <summary>
        ///     Create new instance of <see cref="SoapServiceClient" /> .
        /// </summary>
        /// <param name="endpointConfigurationName">WCF's endpoint name.</param>
        public RestServiceClient(string endpointConfigurationName)
        {
            if (string.IsNullOrWhiteSpace(endpointConfigurationName))
            {
                throw new ConfigurationErrorsException("Invalid endpointConfigurationName: Is null or empty");
            }
            _endpointConfigurationName = endpointConfigurationName;
        }

        public void Delete<TRequest>(TRequest request)
            where TRequest : class
        {
            Process(request, OperationType.Delete);
        }

        public Task DeleteAsync<TRequest>(TRequest request)
            where TRequest : class
        {
            return Task.Run(() => Delete(request));
        }

        public TResponse Get<TRequest, TResponse>(TRequest request)
            where TRequest : class
            where TResponse : class
        {
            return ProcessWithResponse<TRequest, TResponse>(request, OperationType.Get);
        }

        public Task<TResponse> GetAsync<TRequest, TResponse>(TRequest request)
            where TRequest : class
            where TResponse : class
        {
            return Task.Run(() => Get<TRequest, TResponse>(request));
        }

        public void Post<TRequest>(TRequest request)
            where TRequest : class
        {
            Process(request, OperationType.Post);
        }

        public TResponse Post<TRequest, TResponse>(TRequest request)
            where TRequest : class
            where TResponse : class
        {
            return ProcessWithResponse<TRequest, TResponse>(request, OperationType.Post);
        }

        public Task<TResponse> PostAsync<TRequest, TResponse>(TRequest request)
            where TRequest : class
            where TResponse : class
        {
            return Task.Run(() => Post<TRequest, TResponse>(request));
        }

        public Task PostAsync<TRequest>(TRequest request)
            where TRequest : class
        {
            return Task.Run(() => Post(request));
        }

        public void Put<TRequest>(TRequest request)
            where TRequest : class
        {
            Process(request, OperationType.Put);
        }

        public TResponse Put<TRequest, TResponse>(TRequest request)
            where TRequest : class
            where TResponse : class
        {
            return ProcessWithResponse<TRequest, TResponse>(request, OperationType.Put);
        }

        public Task PutAsync<TRequest>(TRequest request)
            where TRequest : class
        {
            return Task.Run(() => Put(request));
        }

        public Task<TResponse> PutAsync<TRequest, TResponse>(TRequest request)
            where TRequest : class
            where TResponse : class
        {
            return Task.Run(() => Put<TRequest, TResponse>(request));
        }

        private static Message AddMessageHeaders<TRequest>(TRequest request, string operationType, Message message)
        {
            var messageProperty = new HttpRequestMessageProperty();

            var typeHeader = new RestContentTypeHeader(typeof(TRequest));
            messageProperty.Headers.Add(typeHeader.Name, typeHeader.Value);

            if (operationType == OperationType.Get)
            {
                var dataHeader = new RestContentDataHeader(request);
                messageProperty.Headers.Add(dataHeader.Name, dataHeader.Value);
            }
            message.Properties.Add(HttpRequestMessageProperty.Name, messageProperty);
            return message;
        }

        private static Message CreateMessage<TRequest>(TRequest request, string operationType)
        {
            var serializer = new DataContractJsonSerializer(typeof(TRequest));
            Message message = Message.CreateMessage(MessageVersion.None, "*", request, serializer);
            message.Properties.Add(
                WebBodyFormatMessageProperty.Name,
                new WebBodyFormatMessageProperty(WebContentFormat.Json));

            message = AddMessageHeaders(request, operationType, message);
            return message;
        }

        private static TResponse GetContent<TResponse>(Message message)
        {
            using (var stream = new MemoryStream())
            {
                XmlDictionaryWriter writer = JsonReaderWriterFactory.CreateJsonWriter(stream);
                message.WriteMessage(writer);
                writer.Flush();
                var serializer = new DataContractJsonSerializer(typeof(TResponse));
                stream.Position = 0;
                return (TResponse)serializer.ReadObject(stream);
            }
        }

        private void Process<TRequest>(TRequest request, string operationType)
            where TRequest : class
        {
            using (var factory = new WebChannelFactory<IRestService>(_endpointConfigurationName))
            {
                Message message = CreateMessage(request, operationType);
                IRestService channel = factory.CreateChannel();
                switch (operationType)
                {
                    case OperationType.Post:
                        channel.Post(message);
                        break;
                    case OperationType.Put:
                        channel.Put(message);
                        break;
                    case OperationType.Delete:
                        channel.Delete(message);
                        break;
                }
            }
        }

        private TResponse ProcessWithResponse<TRequest, TResponse>(TRequest request, string operationType)
            where TRequest : class
            where TResponse : class
        {
            using (var factory = new WebChannelFactory<IRestService>(_endpointConfigurationName))
            {
                Message message = CreateMessage(request, operationType);
                IRestService channel = factory.CreateChannel();
                Message response = null;
                switch (operationType)
                {
                    case OperationType.Get:
                        response = channel.Get(message);
                        break;
                    case OperationType.Post:
                        response = channel.PostWithResponse(message);
                        break;
                    case OperationType.Put:
                        response = channel.PutWithResponse(message);
                        break;
                }
                return GetContent<TResponse>(response);
            }
        }
    }
}
