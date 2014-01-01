using System.ServiceModel.Channels;

namespace Nelibur.ServiceModel.Services.Maps
{
    internal interface IRequestProcessorContext
    {
        Message Process(RequestMetadata metadata);
        void ProcessOneWay(RequestMetadata metadata);
    }
}
