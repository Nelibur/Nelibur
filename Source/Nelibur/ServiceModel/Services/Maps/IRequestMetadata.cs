using System;
using System.ServiceModel.Channels;

namespace Nelibur.ServiceModel.Services.Maps
{
    internal interface IRequestMetadata
    {
        string OperationType { get; }
        Type Type { get; }
        TRequest GetRequest<TRequest>();
        Message GetResponse(object response);
    }
}