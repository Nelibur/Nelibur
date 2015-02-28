using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using Nelibur.ServiceModel.Contracts;
using Nelibur.ServiceModel.Services.Headers;
using Nelibur.Sword.Core;

namespace Nelibur.ServiceModel.Clients
{
    public sealed class SoapServiceClient : IDisposable
    {
        private readonly ChannelFactory<ISoapService> _channelFactory;
        private bool _disposed = false;

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
            _channelFactory = new ChannelFactory<ISoapService>(endpointConfigurationName);
        }

        public void Delete(object request)
        {
            SendOneWay(request, SoapOperationTypeHeader.Delete);
        }

        public TResponse Delete<TResponse>(object request)
        {
            return Send<TResponse>(request, SoapOperationTypeHeader.Delete);
        }

        public Task DeleteAsync(object request)
        {
#if NET_4_0
            return Task.Factory.StartNew(() => SendOneWay(request, SoapOperationTypeHeader.Delete));
#else
            return Task.Run(() => SendOneWay(request, SoapOperationTypeHeader.Delete));
#endif
        }

        public Task<TResponse> DeleteAsync<TResponse>(object request)
        {
#if NET_4_0
            return Task.Factory.StartNew(() => Send<TResponse>(request, SoapOperationTypeHeader.Delete));
#else
            return Task.Run(() => Send<TResponse>(request, SoapOperationTypeHeader.Delete));
#endif
        }

        public void Get(object request)
        {
            SendOneWay(request, SoapOperationTypeHeader.Get);
        }

        public TResponse Get<TResponse>(object request)
        {
            return Send<TResponse>(request, SoapOperationTypeHeader.Get);
        }

        public Task GetAsync(object request)
        {
#if NET_4_0
            return Task.Factory.StartNew(() => SendOneWay(request, SoapOperationTypeHeader.Get));
#else
            return Task.Run(() => SendOneWay(request, SoapOperationTypeHeader.Get));
#endif
        }

        public Task<TResponse> GetAsync<TResponse>(object request)
        {
#if NET_4_0
            return Task.Factory.StartNew(() => Send<TResponse>(request, SoapOperationTypeHeader.Get));
#else
            return Task.Run(() => Send<TResponse>(request, SoapOperationTypeHeader.Get));
#endif
        }

        public void Post(object request)
        {
            SendOneWay(request, SoapOperationTypeHeader.Post);
        }

        public TResponse Post<TResponse>(object request)
        {
            return Send<TResponse>(request, SoapOperationTypeHeader.Post);
        }

        public Task<TResponse> PostAsync<TResponse>(object request)
        {
#if NET_4_0
            return Task.Factory.StartNew(() => Send<TResponse>(request, SoapOperationTypeHeader.Post));
#else
            return Task.Run(() => Send<TResponse>(request, SoapOperationTypeHeader.Post));
#endif
        }

        public Task PostAsync(object request)
        {
#if NET_4_0
            return Task.Factory.StartNew(() => SendOneWay(request, SoapOperationTypeHeader.Post));
#else
            return Task.Run(() => SendOneWay(request, SoapOperationTypeHeader.Post));
#endif
        }

        public void Put(object request)
        {
            SendOneWay(request, SoapOperationTypeHeader.Put);
        }

        public TResponse Put<TResponse>(object request)
        {
            return Send<TResponse>(request, SoapOperationTypeHeader.Put);
        }

        public Task PutAsync(object request)
        {
#if NET_4_0
            return Task.Factory.StartNew(() => Put(request));
#else
            return Task.Run(() => Put(request));
#endif
        }

        public Task<TResponse> PutAsync<TResponse>(object request)
        {
#if NET_4_0
            return Task.Factory.StartNew(() => Send<TResponse>(request, SoapOperationTypeHeader.Put));
#else
            return Task.Run(() => Send<TResponse>(request, SoapOperationTypeHeader.Put));
#endif
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
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

        private static Message CreateOneWayMessage(
            object request, MessageHeader actionHeader, MessageVersion messageVersion)
        {
            Message message = Message.CreateMessage(messageVersion, SoapServiceMetadata.Action.ProcessOneWay, request);
            var contentTypeHeader = new SoapContentTypeHeader(request.GetType());
            message.Headers.Add(contentTypeHeader);
            message.Headers.Add(actionHeader);
            return message;
        }

        private void Dispose(bool disposing)
        {
            if (!disposing || _disposed)
            {
                return;
            }
            if (_channelFactory != null)
            {
                ((IDisposable)_channelFactory).Dispose();
            }
            _disposed = true;
        }

        private TResponse Send<TResponse>(object request, MessageHeader operationType)
        {
            MessageVersion messageVersion = _channelFactory.Endpoint.Binding.MessageVersion;
            Message message = CreateMessage(request, operationType, messageVersion);
            ISoapService channel = _channelFactory.CreateChannel();
            Message result = channel.Process(message);
            return result.GetBody<TResponse>();
        }

        private void SendOneWay(object request, MessageHeader operationType)
        {
            MessageVersion messageVersion = _channelFactory.Endpoint.Binding.MessageVersion;
            Message message = CreateOneWayMessage(request, operationType, messageVersion);
            ISoapService channel = _channelFactory.CreateChannel();
            channel.ProcessOneWay(message);
        }
    }
}
