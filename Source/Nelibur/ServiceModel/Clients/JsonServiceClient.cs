using System;
using System.Collections.Specialized;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using Nelibur.Core.Extensions;
using Nelibur.ServiceModel.Contracts;
using Nelibur.ServiceModel.Serializers;
using Nelibur.ServiceModel.Services.Operations;

namespace Nelibur.ServiceModel.Clients
{
    public sealed class JsonServiceClient : ServiceClient
    {
        private readonly bool _disposeHandler;
        private readonly HttpClientHandler _httpClientHandler;
        private readonly Uri _serviceAddress;

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
            _httpClientHandler = httpClientHandler;
            _disposeHandler = disposeHandler;
        }

        protected override Task DeleteAsyncCore<TRequest>(TRequest request)
        {
            return ProcessAsync(request, OperationType.Delete);
        }

        protected override Task<TResponse> DeleteAsyncCore<TRequest, TResponse>(TRequest request)
        {
            return ProcessWithResponseAsync<TRequest, TResponse>(request, OperationType.Delete);
        }

        protected override void DeleteCore<TRequest>(TRequest request)
        {
            Process(request, OperationType.Delete);
        }

        protected override TResponse DeleteCore<TRequest, TResponse>(TRequest request)
        {
            return ProcessWithResponse<TRequest, TResponse>(request, OperationType.Delete);
        }

        protected override Task GetAsyncCore<TRequest>(TRequest request)
        {
            return ProcessAsync(request, OperationType.Get);
        }

        protected override Task<TResponse> GetAsyncCore<TRequest, TResponse>(TRequest request)
        {
            return ProcessWithResponseAsync<TRequest, TResponse>(request, OperationType.Get);
        }

        protected override TResponse GetCore<TRequest, TResponse>(TRequest request)
        {
            return ProcessWithResponse<TRequest, TResponse>(request, OperationType.Get);
        }

        protected override void GetCore<TRequest>(TRequest request)
        {
            Process(request, OperationType.Get);
        }

        protected override Task<TResponse> PostAsyncCore<TRequest, TResponse>(TRequest request)
        {
            return ProcessWithResponseAsync<TRequest, TResponse>(request, OperationType.Post);
        }

        protected override Task PostAsyncCore<TRequest>(TRequest request)
        {
            return ProcessAsync(request, OperationType.Post);
        }

        protected override TResponse PostCore<TRequest, TResponse>(TRequest request)
        {
            return ProcessWithResponse<TRequest, TResponse>(request, OperationType.Post);
        }

        protected override void PostCore<TRequest>(TRequest request)
        {
            Process(request, OperationType.Post);
        }

        protected override Task PutAsyncCore<TRequest>(TRequest request)
        {
            return ProcessAsync(request, OperationType.Put);
        }

        protected override Task<TResponse> PutAsyncCore<TRequest, TResponse>(TRequest request)
        {
            return ProcessWithResponseAsync<TRequest, TResponse>(request, OperationType.Put);
        }

        protected override TResponse PutCore<TRequest, TResponse>(TRequest request)
        {
            return ProcessWithResponse<TRequest, TResponse>(request, OperationType.Put);
        }

        protected override void PutCore<TRequest>(TRequest request)
        {
            Process(request, OperationType.Put);
        }

        private static StringContent CreateContent<T>(T value)
        {
            string content = JsonDataSerializer.ToString(value);
            return new StringContent(content, Encoding.UTF8, "application/json");
        }

        private static NameValueCollection CreateQueryCollection<TRequest>(TRequest request)
            where TRequest : class
        {
            return UrlSerializer.FromValue(request).QueryParams;
        }

        private static NameValueCollection CreateQueryCollection(Type value)
        {
            return UrlSerializer.FromType(value).QueryParams;
        }

        private HttpClient CreateHttpClient()
        {
            return new HttpClient(_httpClientHandler, _disposeHandler);
        }

        private string CreateUrlRequest<TRequest>(TRequest request, string operationType, bool responseRequired = true)
            where TRequest : class
        {
            var builder = new UriBuilder(_serviceAddress);
            switch (operationType)
            {
                case OperationType.Post:
                    builder = (responseRequired
                        ? builder.AddPath(RestServiceMetadata.Path.PostWithResponse)
                        : builder.AddPath(RestServiceMetadata.Path.Post))
                        .AddQuery(CreateQueryCollection(typeof(TRequest)));
                    break;
                case OperationType.Put:
                    builder = (responseRequired
                        ? builder.AddPath(RestServiceMetadata.Path.PutWithResponse)
                        : builder.AddPath(RestServiceMetadata.Path.Put))
                        .AddQuery(CreateQueryCollection(typeof(TRequest)));
                    break;
                case OperationType.Get:
                    builder = (responseRequired
                        ? builder.AddPath(RestServiceMetadata.Path.GetWithResponse)
                        : builder.AddPath(RestServiceMetadata.Path.Get))
                        .AddQuery(CreateQueryCollection(request));
                    break;
                case OperationType.Delete:
                    builder = (responseRequired
                        ? builder.AddPath(RestServiceMetadata.Path.DeleteWithResponse)
                        : builder.AddPath(RestServiceMetadata.Path.Delete))
                        .AddQuery(CreateQueryCollection(request));
                    break;
                default:
                    string errorMessage = string.Format(
                                                        "OperationType {0} with void return is absent", operationType);
                    throw new InvalidOperationException(errorMessage);
            }
            return builder.Uri.ToString();
        }

        private void Process<TRequest>(TRequest request, string operationType)
            where TRequest : class
        {
            HttpResponseMessage response = ProcessAsync(request, operationType).Result;
            if (!response.IsSuccessStatusCode)
            {
                throw new WebFaultException(response.StatusCode);
            }
        }

        private async Task<HttpResponseMessage> ProcessAsync<TRequest>(TRequest request, string operationType)
            where TRequest : class
        {
            string urlRequest = CreateUrlRequest(request, operationType, responseRequired: false);
            using (HttpClient client = CreateHttpClient())
            {
                switch (operationType)
                {
                    case OperationType.Get:
                        return await client.GetAsync(urlRequest);
                    case OperationType.Post:
                        return await client.PostAsync(urlRequest, CreateContent(request));
                    case OperationType.Put:
                        return await client.PutAsync(urlRequest, CreateContent(request));
                    case OperationType.Delete:
                        return await client.DeleteAsync(urlRequest);
                    default:
                        string errorMessage = string.Format(
                                                            "OperationType {0} with Response return is absent",
                            operationType);
                        throw new InvalidOperationException(errorMessage);
                }
            }
        }

        //http://stackoverflow.com/questions/12739114/asp-net-mvc-4-async-child-action
        private TResponse ProcessWithResponse<TRequest, TResponse>(
            TRequest request, string operationType)
            where TRequest : class
        {
            string urlRequest = CreateUrlRequest(request, operationType);

            using (HttpClient client = CreateHttpClient())
            {
                HttpResponseMessage response;

                switch (operationType)
                {
                    case OperationType.Get:
                        response = client.GetAsync(urlRequest).Result;
                        break;
                    case OperationType.Post:
                        response = client.PostAsync(urlRequest, CreateContent(request)).Result;
                        break;
                    case OperationType.Put:
                        response = client.PutAsync(urlRequest, CreateContent(request)).Result;
                        break;
                    case OperationType.Delete:
                        response = client.DeleteAsync(urlRequest).Result;
                        break;
                    default:
                        string errorMessage = string.Format(
                                                            "OperationType {0} with Response return is absent",
                            operationType);
                        throw new InvalidOperationException(errorMessage);
                }
                if (!response.IsSuccessStatusCode)
                {
                    throw new WebFaultException(response.StatusCode);
                }
                using (Stream stream = response.Content.ReadAsStreamAsync().Result)
                {
                    var serializer = new DataContractJsonSerializer(typeof(TResponse));
                    return (TResponse)serializer.ReadObject(stream);
                }
            }
        }

        private async Task<TResponse> ProcessWithResponseAsync<TRequest, TResponse>(
            TRequest request, string operationType)
            where TRequest : class
        {
            string urlRequest = CreateUrlRequest(request, operationType);

            using (HttpClient client = CreateHttpClient())
            {
                HttpResponseMessage response;

                switch (operationType)
                {
                    case OperationType.Get:
                        response = await client.GetAsync(urlRequest);
                        break;
                    case OperationType.Post:
                        response = await client.PostAsync(urlRequest, CreateContent(request));
                        break;
                    case OperationType.Put:
                        response = await client.PutAsync(urlRequest, CreateContent(request));
                        break;
                    case OperationType.Delete:
                        response = await client.DeleteAsync(urlRequest);
                        break;
                    default:
                        string errorMessage = string.Format(
                                                            "OperationType {0} with Response return is absent",
                            operationType);
                        throw new InvalidOperationException(errorMessage);
                }
                if (!response.IsSuccessStatusCode)
                {
                    throw new WebFaultException(response.StatusCode);
                }
                using (Stream stream = await response.Content.ReadAsStreamAsync())
                {
                    var serializer = new DataContractJsonSerializer(typeof(TResponse));
                    return (TResponse)serializer.ReadObject(stream);
                }
            }
        }
    }
}
