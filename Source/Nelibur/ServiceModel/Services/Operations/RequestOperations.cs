namespace Nelibur.ServiceModel.Services.Operations
{
    public interface IDeleteOneWay<in TRequest> : IRequestOperation
        where TRequest : class
    {
        void DeleteOneWay(TRequest request);
    }

    public interface IPostOneWay<in TRequest> : IRequestOperation
        where TRequest : class
    {
        void PostOneWay(TRequest request);
    }

    public interface IPutOneWay<in TRequest> : IRequestOperation
        where TRequest : class
    {
        void PutOneWay(TRequest request);
    }

    public interface IDelete<in TRequest> : IRequestOperation
        where TRequest : class
    {
        object Delete(TRequest request);
    }

    public interface IGet<in TRequest> : IRequestOperation
        where TRequest : class
    {
        object Get(TRequest request);
    }

    public interface IPost<in TRequest> : IRequestOperation
        where TRequest : class
    {
        object Post(TRequest request);
    }

    public interface IPut<in TRequest> : IRequestOperation
        where TRequest : class
    {
        object Put(TRequest request);
    }
}
