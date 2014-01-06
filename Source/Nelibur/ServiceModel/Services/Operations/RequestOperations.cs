namespace Nelibur.ServiceModel.Services.Operations
{
    /// <summary>
    ///     Marker interface.
    /// </summary>
    public interface IRequestOperation
    {
    }

    public interface IDelete<in TRequest> : IRequestOperation
        where TRequest : class
    {
        void Delete(TRequest request);
    }

    public interface IGet<in TRequest> : IRequestOperation
        where TRequest : class
    {
        void Get(TRequest request);
    }

    public interface IPost<in TRequest> : IRequestOperation
        where TRequest : class
    {
        void Post(TRequest request);
    }

    public interface IPut<in TRequest> : IRequestOperation
        where TRequest : class
    {
        void Put(TRequest request);
    }

    public interface IDeleteWithResponse<in TRequest> : IRequestOperation
        where TRequest : class
    {
        object DeleteWithResponse(TRequest request);
    }

    public interface IGetWithResponse<in TRequest> : IRequestOperation
        where TRequest : class
    {
        object GetWithResponse(TRequest request);
    }

    public interface IPostWithResponse<in TRequest> : IRequestOperation
        where TRequest : class
    {
        object PostWithResponse(TRequest request);
    }

    public interface IPutWithResponse<in TRequest> : IRequestOperation
        where TRequest : class
    {
        object PutWithResponse(TRequest request);
    }
}
