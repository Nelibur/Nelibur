using System;
using Nelibur.ServiceModel.Services.Operations;

namespace Nelibur.ServiceModel.Services.Processors
{
    public interface IConfiguration
    {
        void Bind<TRequest, TProcessor>(Func<TProcessor> creator)
            where TRequest : class
            where TProcessor : IRequestOperation;

        void Bind<TRequest, TProcessor>()
            where TRequest : class
            where TProcessor : IRequestOperation, new();
    }
}
