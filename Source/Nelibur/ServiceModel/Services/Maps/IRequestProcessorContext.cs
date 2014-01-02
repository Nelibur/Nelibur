using System.ServiceModel.Channels;

namespace Nelibur.ServiceModel.Services.Maps
{
    internal interface IRequestProcessorContext
    {
        void Process(RequestMetadata metadata);
        Message ProcessWithResponse(RequestMetadata metadata);
    }
}
