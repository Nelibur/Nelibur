using System;
using System.Collections.Specialized;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;
using Nelibur.Core.Extensions;
using Nelibur.ServiceModel.Contracts;
using Nelibur.ServiceModel.Services.Headers;
using Nelibur.ServiceModel.Services.Operations;

namespace Nelibur.ServiceModel.Clients
{
    public sealed class JsonServiceClient : ServiceClient
    {
        private readonly HttpClientHandler _httpClientHandler;
        private readonly Uri _serviceAddress;

        public JsonServiceClient(string serviceAddress) : this(serviceAddress, new HttpClientHandler())
        {
        }

        public JsonServiceClient(string serviceAddress, HttpClientHandler httpClientHandler)
        {
            _serviceAddress = new Uri(serviceAddress);
            _httpClientHandler = httpClientHandler;
        }

        protected override void DeleteCore<TRequest>(TRequest request)
        {
            Process(request, OperationType.Delete).Wait();
        }

        protected override TResponse DeleteCore<TRequest, TResponse>(TRequest request)
        {
            return ProcessWithResponse<TRequest, TResponse>(request, OperationType.Delete).Result;
        }

        protected override TResponse GetCore<TRequest, TResponse>(TRequest request)
        {
            return ProcessWithResponse<TRequest, TResponse>(request, OperationType.Get).Result;
        }

        protected override void GetCore<TRequest>(TRequest request)
        {
            Process(request, OperationType.Get).Wait();
        }

        protected override void PostCore<TRequest>(TRequest request)
        {
            Process(request, OperationType.Post).Wait();
        }

        protected override TResponse PostCore<TRequest, TResponse>(TRequest request)
        {
            return ProcessWithResponse<TRequest, TResponse>(request, OperationType.Post).Result;
        }

        protected override void PutCore<TRequest>(TRequest request)
        {
            Process(request, OperationType.Put).Wait();
        }

        protected override TResponse PutCore<TRequest, TResponse>(TRequest request)
        {
            return ProcessWithResponse<TRequest, TResponse>(request, OperationType.Put).Result;
        }

        private static StringContent CreateContent<T>(T value)
        {
            using (var stream = new MemoryStream())
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                serializer.WriteObject(stream, value);
                string content = Encoding.UTF8.GetString(stream.ToArray());
                var result = new StringContent(content, Encoding.UTF8, "application/json");
                return result;
            }
        }

        private static NameValueCollection CreateQueryCollection<TRequest>(TRequest request)
            where TRequest : class
        {
            string requestValue;
            using (var stream = new MemoryStream())
            {
                var serializer = new DataContractJsonSerializer(typeof(TRequest));
                serializer.WriteObject(stream, request);
                var converter = new QueryStringConverter();
                requestValue = converter.ConvertValueToString(stream.ToArray(), typeof(byte[]));
            }
            return new NameValueCollection { { RestServiceMetadata.ParamNames.Request, requestValue } };
        }

        private HttpClient CreateHttpClient<TRequest>()
            where TRequest : class
        {
            var client = new HttpClient(_httpClientHandler);
            var typeHeader = new RestContentTypeHeader(typeof(TRequest));
            client.DefaultRequestHeaders.Add(typeHeader.Name, typeHeader.Value);
            return client;
        }

        private string CreateUrlRequest<TRequest>(TRequest request, string operationType, bool responseRequired = true)
            where TRequest : class
        {
            var builder = new UriBuilder(_serviceAddress);
            switch (operationType)
            {
                case OperationType.Post:
                    builder = responseRequired
                        ? builder.AddPath(RestServiceMetadata.Operations.PostWithResponse)
                        : builder.AddPath(RestServiceMetadata.Operations.Post);
                    break;
                case OperationType.Put:
                    builder = responseRequired
                        ? builder.AddPath(RestServiceMetadata.Operations.PutWithResponse)
                        : builder.AddPath(RestServiceMetadata.Operations.Put);
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
            HttpClient client = CreateHttpClient<TRequest>();
            string urlRequest = CreateUrlRequest(request, operationType, responseRequired: false);

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

        private async Task<TResponse> ProcessWithResponse<TRequest, TResponse>(TRequest request, string operationType)
            where TRequest : class
            where TResponse : class
        {
            HttpClient client = CreateHttpClient<TRequest>();
            string urlRequest = CreateUrlRequest(request, operationType);

            Task<HttpResponseMessage> responseTask;

            switch (operationType)
            {
                case OperationType.Get:
                    responseTask = client.GetAsync(urlRequest);
                    break;
                case OperationType.Post:
                    responseTask = client.PostAsync(urlRequest, CreateContent(request));
                    break;
                case OperationType.Put:
                    responseTask = client.PutAsync(urlRequest, CreateContent(request));
                    break;
                case OperationType.Delete:
                    responseTask = client.DeleteAsync(urlRequest);
                    break;
                default:
                    string errorMessage = string.Format(
                        "OperationType {0} with Response return is absent", operationType);
                    throw new InvalidOperationException(errorMessage);
            }
            using (Stream stream = await (await responseTask).Content.ReadAsStreamAsync())
            {
                var serializer = new DataContractJsonSerializer(typeof(TResponse));
                return (TResponse)serializer.ReadObject(stream);
            }
        }
    }
}
