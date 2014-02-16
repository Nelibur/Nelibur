using System.Threading.Tasks;

namespace Nelibur.ServiceModel.Clients
{
    public abstract class ServiceClient
    {
        public void Delete<TRequest>(TRequest request)
            where TRequest : class
        {
            DeleteCore(request);
        }

        public TResponse Delete<TRequest, TResponse>(TRequest request)
            where TRequest : class
        {
            return DeleteAsyncCore<TRequest, TResponse>(request).Result;
        }

        public Task DeleteAsync<TRequest>(TRequest request)
            where TRequest : class
        {
            return DeleteAsyncCore(request);
        }

        public Task<TResponse> DeleteAsync<TRequest, TResponse>(TRequest request)
            where TRequest : class
        {
            return DeleteAsyncCore<TRequest, TResponse>(request);
        }

        public void Get<TRequest>(TRequest request)
            where TRequest : class
        {
            GetCore(request);
        }

        public TResponse Get<TRequest, TResponse>(TRequest request)
            where TRequest : class
        {
            return GetCore<TRequest, TResponse>(request);
        }

        public Task GetAsync<TRequest>(TRequest request)
            where TRequest : class
        {
            return GetAsyncCore(request);
        }

        public Task<TResponse> GetAsync<TRequest, TResponse>(TRequest request)
            where TRequest : class
        {
            return GetAsyncCore<TRequest, TResponse>(request);
        }

        public void Post<TRequest>(TRequest request)
            where TRequest : class
        {
            PostCore(request);
        }

        public TResponse Post<TRequest, TResponse>(TRequest request)
            where TRequest : class
        {
            return PostAsyncCore<TRequest, TResponse>(request).Result;
        }

        public Task<TResponse> PostAsync<TRequest, TResponse>(TRequest request)
            where TRequest : class
        {
            return PostAsyncCore<TRequest, TResponse>(request);
        }

        public Task PostAsync<TRequest>(TRequest request)
            where TRequest : class
        {
            return PostAsyncCore(request);
        }

        public void Put<TRequest>(TRequest request)
            where TRequest : class
        {
            PutCore(request);
        }

        public TResponse Put<TRequest, TResponse>(TRequest request)
            where TRequest : class
        {
            return PutAsyncCore<TRequest, TResponse>(request).Result;
        }

        public Task PutAsync<TRequest>(TRequest request)
            where TRequest : class
        {
            return PutAsyncCore(request);
        }

        public Task<TResponse> PutAsync<TRequest, TResponse>(TRequest request)
            where TRequest : class
        {
            return PutAsyncCore<TRequest, TResponse>(request);
        }

        protected abstract Task DeleteAsyncCore<TRequest>(TRequest request)
            where TRequest : class;

        protected abstract Task<TResponse> DeleteAsyncCore<TRequest, TResponse>(TRequest request)
            where TRequest : class;

        protected abstract void DeleteCore<TRequest>(TRequest request)
            where TRequest : class;

        protected abstract Task GetAsyncCore<TRequest>(TRequest request)
            where TRequest : class;

        protected abstract Task<TResponse> GetAsyncCore<TRequest, TResponse>(TRequest request)
            where TRequest : class;

        protected abstract TResponse GetCore<TRequest, TResponse>(TRequest request)
            where TRequest : class;

        protected abstract void GetCore<TRequest>(TRequest request)
            where TRequest : class;

        protected abstract Task<TResponse> PostAsyncCore<TRequest, TResponse>(TRequest request)
            where TRequest : class;

        protected abstract Task PostAsyncCore<TRequest>(TRequest request)
            where TRequest : class;

        protected abstract void PostCore<TRequest>(TRequest request)
            where TRequest : class;

        protected abstract Task PutAsyncCore<TRequest>(TRequest request)
            where TRequest : class;

        protected abstract Task<TResponse> PutAsyncCore<TRequest, TResponse>(TRequest request)
            where TRequest : class;

        protected abstract void PutCore<TRequest>(TRequest request)
            where TRequest : class;
    }
}
