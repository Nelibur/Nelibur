using System;
using System.ServiceModel.Channels;

namespace Nelibur.ServiceModel.Services.Maps
{
    internal interface IRequestProcessor
    {
        Message Process(RequestMetadata metadata);
        void ProcessOneWay(RequestMetadata metadata);
    }
}
