using System;
using System.Collections.Specialized;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
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
            return Process(request, OperationType.Delete);
        }

        protected override Task<TResponse> DeleteAsyncCore<TRequest, TResponse>(TRequest request)
        {
            return ProcessWithResponse<TRequest, TResponse>(request, OperationType.Delete);
        }

        protected override void DeleteCore<TRequest>(TRequest request)
        {
            DeleteAsyncCore(request).Wait();
        }

        protected override Task GetAsyncCore<TRequest>(TRequest request)
        {
            return Process(request, OperationType.Get);
        }

        protected override Task<TResponse> GetAsyncCore<TRequest, TResponse>(TRequest request)
        {
            return ProcessWithResponse<TRequest, TResponse>(request, OperationType.Get);
        }

        protected override TResponse GetCore<TRequest, TResponse>(TRequest request)
        {
            return GetAsyncCore<TRequest, TResponse>(request).Result;
        }

        protected override void GetCore<TRequest>(TRequest request)
        {
            GetAsyncCore(request).Wait();
        }

        protected override Task<TResponse> PostAsyncCore<TRequest, TResponse>(TRequest request)
        {
            return ProcessWithResponse<TRequest, TResponse>(request, OperationType.Post);
        }

        protected override Task PostAsyncCore<TRequest>(TRequest request)
        {
            return Process(request, OperationType.Post);
        }

        protected override void PostCore<TRequest>(TRequest request)
        {
            PostAsyncCore(request).Wait();
        }

        protected override Task PutAsyncCore<TRequest>(TRequest request)
        {
            return Process(request, OperationType.Put);
        }

        protected override Task<TResponse> PutAsyncCore<TRequest, TResponse>(TRequest request)
        {
            return ProcessWithResponse<TRequest, TResponse>(request, OperationType.Put);
        }

        protected override void PutCore<TRequest>(TRequest request)
        {
            PutAsyncCore(request).Wait();
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
                        ? builder.AddPath(RestServiceMetadata.Operations.PostWithResponse)
                        : builder.AddPath(RestServiceMetadata.Operations.Post))
                        .AddQuery(CreateQueryCollection(typeof(TRequest)));
                    break;
                case OperationType.Put:
                    builder = (responseRequired
                        ? builder.AddPath(RestServiceMetadata.Operations.PutWithResponse)
                        : builder.AddPath(RestServiceMetadata.Operations.Put))
                        .AddQuery(CreateQueryCollection(typeof(TRequest)));
                    break;
                case OperationType.Get:
                    builder = (responseRequired
                        ? builder.AddPath(RestServiceMetadata.Operations.GetWithResponse)
                        : builder.AddPath(RestServiceMetadata.Operations.Get))
                        .AddQuery(CreateQueryCollection(request));
                    break;
                case OperationType.Delete:
                    builder = (responseRequired
                        ? builder.AddPath(RestServiceMetadata.Operations.DeleteWithResponse)
                        : builder.AddPath(RestServiceMetadata.Operations.Delete))
                        .AddQuery(CreateQueryCollection(request));
                    break;
                default:
                    string errorMessage = string.Format(
                        "OperationType {0} with void return is absent", operationType);
                    throw new InvalidOperationException(errorMessage);
            }
            return builder.Uri.ToString();
        }

        private async Task Process<TRequest>(TRequest request, string operationType)
            where TRequest : class
        {
            string urlRequest = CreateUrlRequest(request, operationType, responseRequired: false);
            using (HttpClient client = CreateHttpClient())
            {
                switch (operationType)
                {
                    case OperationType.Get:
                        await client.GetAsync(urlRequest);
                        break;
                    case OperationType.Post:
                        await client.PostAsync(urlRequest, CreateContent(request));
                        break;
                    case OperationType.Put:
                        await client.PutAsync(urlRequest, CreateContent(request));
                        break;
                    case OperationType.Delete:
                        await client.DeleteAsync(urlRequest);
                        break;
                    default:
                        string errorMessage = string.Format(
                            "OperationType {0} with Response return is absent", operationType);
                        throw new InvalidOperationException(errorMessage);
                }
            }
        }

        private async Task<TResponse> ProcessWithResponse<TRequest, TResponse>(TRequest request, string operationType)
            where TRequest : class
            where TResponse : class
        {
            string urlRequest = CreateUrlRequest(request, operationType);

            using (HttpClient client = CreateHttpClient())
            {
                HttpResponseMessage responseTask;

                switch (operationType)
                {
                    case OperationType.Get:
                        responseTask = await client.GetAsync(urlRequest);
                        break;
                    case OperationType.Post:
                        responseTask = await client.PostAsync(urlRequest, CreateContent(request));
                        break;
                    case OperationType.Put:
                        responseTask = await client.PutAsync(urlRequest, CreateContent(request));
                        break;
                    case OperationType.Delete:
                        responseTask = await client.DeleteAsync(urlRequest);
                        break;
                    default:
                        string errorMessage = string.Format(
                            "OperationType {0} with Response return is absent", operationType);
                        throw new InvalidOperationException(errorMessage);
                }
                using (Stream stream = await responseTask.Content.ReadAsStreamAsync())
                {
                    var serializer = new DataContractJsonSerializer(typeof(TResponse));
                    return (TResponse)serializer.ReadObject(stream);
                }
            }
        }
    }
}
