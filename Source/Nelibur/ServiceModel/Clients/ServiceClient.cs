using System.Configuration;
using System.Threading.Tasks;

namespace Nelibur.ServiceModel.Clients
{
    public abstract class ServiceClient
    {
        protected readonly string _endpointConfigurationName;

        /// <summary>
        ///     Create new instance of <see cref="SoapServiceClient" /> .
        /// </summary>
        /// <param name="endpointConfigurationName">WCF's endpoint name.</param>
        protected ServiceClient(string endpointConfigurationName)
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
            DeleteCore(request);
        }

        public Task DeleteAsync<TRequest>(TRequest request)
            where TRequest : class
        {
            return Task.Run(() => Delete(request));
        }

        public Task<TResponse> DeleteAsync<TRequest, TResponse>(TRequest request)
            where TRequest : class
            where TResponse : class
        {
            return Task.Run(() => DeleteCore<TRequest, TResponse>(request));
        }

        public void Get<TRequest>(TRequest request)
            where TRequest : class
        {
            GetCore(request);
        }

        public TResponse Get<TRequest, TResponse>(TRequest request)
            where TRequest : class
            where TResponse : class
        {
            return GetCore<TRequest, TResponse>(request);
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
            PostCore(request);
        }

        public TResponse Post<TRequest, TResponse>(TRequest request)
            where TRequest : class
            where TResponse : class
        {
            return PostCore<TRequest, TResponse>(request);
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
            PutCore(request);
        }

        public TResponse Put<TRequest, TResponse>(TRequest request)
            where TRequest : class
            where TResponse : class
        {
            return PutCore<TRequest, TResponse>(request);
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

        protected abstract void DeleteCore<TRequest>(TRequest request)
            where TRequest : class;

        protected abstract TResponse DeleteCore<TRequest, TResponse>(TRequest request)
            where TRequest : class
            where TResponse : class;

        protected abstract TResponse GetCore<TRequest, TResponse>(TRequest request)
            where TRequest : class
            where TResponse : class;

        protected abstract void GetCore<TRequest>(TRequest request)
            where TRequest : class;

        protected abstract void PostCore<TRequest>(TRequest request)
            where TRequest : class;

        protected abstract TResponse PostCore<TRequest, TResponse>(TRequest request)
            where TRequest : class
            where TResponse : class;

        protected abstract void PutCore<TRequest>(TRequest request)
            where TRequest : class;

        protected abstract TResponse PutCore<TRequest, TResponse>(TRequest request)
            where TRequest : class
            where TResponse : class;
    }
}
