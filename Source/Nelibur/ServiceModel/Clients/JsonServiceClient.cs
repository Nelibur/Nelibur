using System;
using System.IO;
using System.Net.Http;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using Nelibur.ServiceModel.Extensions;
using Nelibur.ServiceModel.Serializers;
using Nelibur.ServiceModel.Services.Operations;
using Nelibur.Sword.Core;

namespace Nelibur.ServiceModel.Clients
{
    public sealed class JsonServiceClient : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly Uri _serviceAddress;
        private bool _disposed = false;

        /// <summary>
        ///     Create new instanse of <see cref="JsonServiceClient" />.
        /// </summary>
        /// <param name="serviceAddress">Service address.</param>
        /// <remarks>
        ///     Default <see cref="HttpClientHandler" /> and DisposeHandler is equal to false.
        /// </remarks>
        public JsonServiceClient(string serviceAddress) : this(serviceAddress, new HttpClientHandler(), false)
        {
        }

        /// <summary>
        ///     Create new instanse of <see cref="JsonServiceClient" />.
        /// </summary>
        /// <param name="serviceAddress">Service address.</param>
        /// <param name="httpClientHandler">
        ///     The <see cref="HttpMessageHandler" /> responsible for processing the HTTP response message.
        /// </param>
        /// <param name="disposeHandler">
        ///     True if the inner handler should be disposed of by Dispose(),false if you intend to reuse the inner handler.
        /// </param>
        public JsonServiceClient(string serviceAddress, HttpClientHandler httpClientHandler, bool disposeHandler)
        {
            _serviceAddress = new Uri(serviceAddress);
            _httpClient = new HttpClient(httpClientHandler, disposeHandler);
        }

        public void Delete(object request)
        {
            SendOneWay(request, OperationType.Delete, false);
        }

        public TResponse Delete<TResponse>(object request)
        {
            return Send<TResponse>(request, OperationType.Delete);
        }

        public Task DeleteAsync(object request)
        {
            return SendOneWayAsync(request, OperationType.Delete);
        }

        public Task<TResponse> DeleteAsync<TResponse>(object request)
        {
            return SendAsync<TResponse>(request, OperationType.Delete);
        }

        public void Get(object request)
        {
            SendOneWay(request, OperationType.Get, false);
        }

        public TResponse Get<TResponse>(object request)
        {
            return Send<TResponse>(request, OperationType.Get);
        }

        public Task GetAsync(object request)
        {
            return SendOneWayAsync(request, OperationType.Get);
        }

        public Task<TResponse> GetAsync<TResponse>(object request)
        {
            return SendAsync<TResponse>(request, OperationType.Get);
        }

        public void Post(object request)
        {
            SendOneWay(request, OperationType.Post, false);
        }

        public TResponse Post<TResponse>(object request)
        {
            return Send<TResponse>(request, OperationType.Post);
        }

        public Task<TResponse> PostAsync<TResponse>(object request)
        {
            return SendAsync<TResponse>(request, OperationType.Post);
        }

        public Task PostAsync(object request)
        {
            return SendOneWayAsync(request, OperationType.Post);
        }

        public void Put(object request)
        {
            SendOneWay(request, OperationType.Put, false);
        }

        public TResponse Put<TResponse>(object request)
        {
            return Send<TResponse>(request, OperationType.Put);
        }

        public Task PutAsync(object request)
        {
            return SendOneWayAsync(request, OperationType.Put);
        }

        public Task<TResponse> PutAsync<TResponse>(object request)
        {
            return SendAsync<TResponse>(request, OperationType.Put);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private static StringContent CreateContent(object value)
        {
            string content = JsonDataSerializer.ToString(value);
            return new StringContent(content, Encoding.UTF8, "application/json");
        }

        private void Dispose(bool disposing)
        {
            if (!disposing || _disposed)
            {
                return;
            }
            if (_httpClient != null)
            {
                _httpClient.Dispose();
            }
            _disposed = true;
        }

        //http://stackoverflow.com/questions/12739114/asp-net-mvc-4-async-child-action
        private TResponse Send<TResponse>(object request, string operationType)
        {
            HttpResponseMessage response = SendOneWay(request, operationType);
            using (Stream stream = response.Content.ReadAsStreamAsync().Result)
            {
                return JsonDataSerializer.ToValue<TResponse>(stream);
            }
        }

        private async Task<TResponse> SendAsync<TResponse>(
            object request, string operationType)
        {
            string urlRequest = request.ToUrl(_serviceAddress, operationType);

            HttpResponseMessage response;
            switch (operationType)
            {
                case OperationType.Get:
                    response = await _httpClient.GetAsync(urlRequest);
                    break;
                case OperationType.Post:
                    response = await _httpClient.PostAsync(urlRequest, CreateContent(request));
                    break;
                case OperationType.Put:
                    response = await _httpClient.PutAsync(urlRequest, CreateContent(request));
                    break;
                case OperationType.Delete:
                    response = await _httpClient.DeleteAsync(urlRequest);
                    break;
                default:
                    string errorMessage = string.Format("OperationType {0} with Response return is absent",
                        operationType);
                    throw Error.InvalidOperation(errorMessage);
            }
            if (!response.IsSuccessStatusCode)
            {
                throw new WebFaultException(response.StatusCode);
            }
            using (Stream stream = await response.Content.ReadAsStreamAsync())
            {
                return JsonDataSerializer.ToValue<TResponse>(stream);
            }
        }

        private HttpResponseMessage SendOneWay(object request, string operationType, bool responseRequired = true)
        {
            string urlRequest = request.ToUrl(_serviceAddress, operationType, responseRequired);
            HttpResponseMessage response;
            switch (operationType)
            {
                case OperationType.Get:
                    response = _httpClient.GetAsync(urlRequest).Result;
                    break;
                case OperationType.Post:
                    response = _httpClient.PostAsync(urlRequest, CreateContent(request)).Result;
                    break;
                case OperationType.Put:
                    response = _httpClient.PutAsync(urlRequest, CreateContent(request)).Result;
                    break;
                case OperationType.Delete:
                    response = _httpClient.DeleteAsync(urlRequest).Result;
                    break;
                default:
                    string errorMessage = string.Format("OperationType {0} with Response return is absent",
                        operationType);
                    throw Error.InvalidOperation(errorMessage);
            }
            if (!response.IsSuccessStatusCode)
            {
                throw new WebFaultException(response.StatusCode);
            }
            return response;
        }

        private async Task<HttpResponseMessage> SendOneWayAsync(object request, string operationType)
        {
            string urlRequest = request.ToUrl(_serviceAddress, operationType, false);
            switch (operationType)
            {
                case OperationType.Get:
                    return await _httpClient.GetAsync(urlRequest);
                case OperationType.Post:
                    return await _httpClient.PostAsync(urlRequest, CreateContent(request));
                case OperationType.Put:
                    return await _httpClient.PutAsync(urlRequest, CreateContent(request));
                case OperationType.Delete:
                    return await _httpClient.DeleteAsync(urlRequest);
                default:
                    string errorMessage = string.Format(
                        "OperationType {0} with Response return is absent",
                        operationType);
                    throw Error.InvalidOperation(errorMessage);
            }
        }
    }
}
