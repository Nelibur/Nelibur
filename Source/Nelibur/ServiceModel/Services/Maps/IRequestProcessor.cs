using System;
using System.ServiceModel.Channels;

namespace Nelibur.ServiceModel.Services.Maps
{
    internal interface IRequestProcessor
    {
        void Process(RequestMetadata metadata);
        Message ProcessWithResponse(RequestMetadata metadata);
    }
}
