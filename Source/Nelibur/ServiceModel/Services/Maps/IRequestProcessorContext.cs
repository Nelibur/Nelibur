using System.ServiceModel.Channels;

namespace Nelibur.ServiceModel.Services.Maps
{
    internal interface IRequestProcessorContext
    {
        Message ProcessWithResponse(RequestMetadata metadata);
        void Process(RequestMetadata metadata);
    }
}
