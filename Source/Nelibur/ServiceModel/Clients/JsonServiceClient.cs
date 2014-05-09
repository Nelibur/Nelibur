﻿using System;
using System.IO;
using System.Net.Http;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using Nelibur.Core;
using Nelibur.ServiceModel.Extensions;
using Nelibur.ServiceModel.Serializers;
using Nelibur.ServiceModel.Services.Operations;

namespace Nelibur.ServiceModel.Clients
{
    public sealed class JsonServiceClient
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

        public void Delete<TRequest>(TRequest request)
            where TRequest : class
        {
            Process(request, OperationType.Delete, false);
        }

        public TResponse Delete<TRequest, TResponse>(TRequest request)
            where TRequest : class
        {
            return ProcessWithResponse<TRequest, TResponse>(request, OperationType.Delete);
        }

        public Task DeleteAsync<TRequest>(TRequest request)
            where TRequest : class
        {
            return ProcessAsync(request, OperationType.Delete);
        }

        public Task<TResponse> DeleteAsync<TRequest, TResponse>(TRequest request)
            where TRequest : class
        {
            return ProcessWithResponseAsync<TRequest, TResponse>(request, OperationType.Delete);
        }

        public void Get<TRequest>(TRequest request)
            where TRequest : class
        {
            Process(request, OperationType.Get, false);
        }

        public TResponse Get<TRequest, TResponse>(TRequest request)
            where TRequest : class
        {
            return ProcessWithResponse<TRequest, TResponse>(request, OperationType.Get);
        }

        public Task GetAsync<TRequest>(TRequest request)
            where TRequest : class
        {
            return ProcessAsync(request, OperationType.Get);
        }

        public Task<TResponse> GetAsync<TRequest, TResponse>(TRequest request)
            where TRequest : class
        {
            return ProcessWithResponseAsync<TRequest, TResponse>(request, OperationType.Get);
        }

        public void Post<TRequest>(TRequest request)
            where TRequest : class
        {
            Process(request, OperationType.Post, false);
        }

        public TResponse Post<TRequest, TResponse>(TRequest request)
            where TRequest : class
        {
            return ProcessWithResponse<TRequest, TResponse>(request, OperationType.Post);
        }

        public Task<TResponse> PostAsync<TRequest, TResponse>(TRequest request)
            where TRequest : class
        {
            return ProcessWithResponseAsync<TRequest, TResponse>(request, OperationType.Post);
        }

        public Task PostAsync<TRequest>(TRequest request)
            where TRequest : class
        {
            return ProcessAsync(request, OperationType.Post);
        }

        public void Put<TRequest>(TRequest request)
            where TRequest : class
        {
            Process(request, OperationType.Put, false);
        }

        public TResponse Put<TRequest, TResponse>(TRequest request)
            where TRequest : class
        {
            return ProcessWithResponse<TRequest, TResponse>(request, OperationType.Put);
        }

        public Task PutAsync<TRequest>(TRequest request)
            where TRequest : class
        {
            return ProcessAsync(request, OperationType.Put);
        }

        public Task<TResponse> PutAsync<TRequest, TResponse>(TRequest request)
            where TRequest : class
        {
            return ProcessWithResponseAsync<TRequest, TResponse>(request, OperationType.Put);
        }

        private static StringContent CreateContent<T>(T value)
        {
            string content = JsonDataSerializer.ToString(value);
            return new StringContent(content, Encoding.UTF8, "application/json");
        }

        private HttpClient CreateHttpClient()
        {
            return new HttpClient(_httpClientHandler, _disposeHandler);
        }

        private HttpResponseMessage Process<TRequest>(TRequest request, string operationType, bool responseRequired = true)
            where TRequest : class
        {
            string urlRequest = request.ToUrl(_serviceAddress, operationType, responseRequired);
            HttpResponseMessage response;

            using (HttpClient client = CreateHttpClient())
            {
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
                        string errorMessage = string.Format("OperationType {0} with Response return is absent",
                            operationType);
                        throw Error.InvalidOperation(errorMessage);
                }
                if (!response.IsSuccessStatusCode)
                {
                    throw new WebFaultException(response.StatusCode);
                }
            }
            return response;
        }

        private async Task<HttpResponseMessage> ProcessAsync<TRequest>(TRequest request, string operationType)
            where TRequest : class
        {
            string urlRequest = request.ToUrl(_serviceAddress, operationType, false);

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
                        throw Error.InvalidOperation(errorMessage);
                }
            }
        }

        //http://stackoverflow.com/questions/12739114/asp-net-mvc-4-async-child-action
        private TResponse ProcessWithResponse<TRequest, TResponse>(TRequest request, string operationType)
            where TRequest : class
        {
            HttpResponseMessage response = Process(request, operationType);
            using (Stream stream = response.Content.ReadAsStreamAsync().Result)
            {
                return JsonDataSerializer.ToValue<TResponse>(stream);
            }
        }

        private async Task<TResponse> ProcessWithResponseAsync<TRequest, TResponse>(
            TRequest request, string operationType)
            where TRequest : class
        {
            string urlRequest = request.ToUrl(_serviceAddress, operationType);

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
        }
    }
}
