using System.ServiceModel.Channels;

namespace Nelibur.ServiceModel.Services.Maps
{
    internal interface IRequestProcessorContext
    {
        void Process(IRequestMetadata metadata);
        Message ProcessWithResponse(IRequestMetadata metadata);
    }
}
