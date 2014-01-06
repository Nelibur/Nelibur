using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Xml;
using Nelibur.ServiceModel.Contracts;
using Nelibur.ServiceModel.Services.Headers;
using Nelibur.ServiceModel.Services.Operations;

namespace Nelibur.ServiceModel.Clients
{
    public sealed class RestServiceClient : ServiceClient
    {
        /// <summary>
        ///     Create new instance of <see cref="SoapServiceClient" /> .
        /// </summary>
        /// <param name="endpointConfigurationName">WCF's endpoint name.</param>
        public RestServiceClient(string endpointConfigurationName) : base(endpointConfigurationName)
        {
        }

        protected override void DeleteCore<TRequest>(TRequest request)
        {
            Process(request, OperationType.Delete);
        }

        protected override TResponse DeleteCore<TRequest, TResponse>(TRequest request)
        {
            return ProcessWithResponse<TRequest, TResponse>(request, OperationType.Delete);
        }

        protected override TResponse GetCore<TRequest, TResponse>(TRequest request)
        {
            return ProcessWithResponse<TRequest, TResponse>(request, OperationType.Get);
        }

        protected override void GetCore<TRequest>(TRequest request)
        {
            Process(request, OperationType.Get);
        }

        protected override void PostCore<TRequest>(TRequest request)
        {
            Process(request, OperationType.Post);
        }

        protected override TResponse PostCore<TRequest, TResponse>(TRequest request)
        {
            return ProcessWithResponse<TRequest, TResponse>(request, OperationType.Post);
        }

        protected override void PutCore<TRequest>(TRequest request)
        {
            Process(request, OperationType.Put);
        }

        protected override TResponse PutCore<TRequest, TResponse>(TRequest request)
        {
            return ProcessWithResponse<TRequest, TResponse>(request, OperationType.Put);
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
            var bodyFormat = new WebBodyFormatMessageProperty(WebContentFormat.Json);

            Message message = Message.CreateMessage(MessageVersion.None, "*", request, serializer);
            message.Properties.Add(WebBodyFormatMessageProperty.Name, bodyFormat);
            message = AddMessageHeaders(request, operationType, message);
            return message;
        }

        private static TResponse GetResponse<TResponse>(Message message)
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
                    case OperationType.Get:
                        channel.Get(message);
                        break;
                    case OperationType.Post:
                        channel.Post(message);
                        break;
                    case OperationType.Put:
                        channel.Put(message);
                        break;
                    case OperationType.Delete:
                        channel.Delete(message);
                        break;
                    default:
                        string errorMessage = string.Format(
                            "OperationType {0} with void return is absent", operationType);
                        throw new InvalidOperationException(errorMessage);
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
                Message response;
                switch (operationType)
                {
                    case OperationType.Get:
                        response = channel.GetWithResponse(message);
                        break;
                    case OperationType.Post:
                        response = channel.PostWithResponse(message);
                        break;
                    case OperationType.Put:
                        response = channel.PutWithResponse(message);
                        break;
                    case OperationType.Delete:
                        response = channel.PutWithResponse(message);
                        break;
                    default:
                        string errorMessage = string.Format(
                            "OperationType {0} with Response return is absent", operationType);
                        throw new InvalidOperationException(errorMessage);
                }
                return GetResponse<TResponse>(response);
            }
        }
    }
}
