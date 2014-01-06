using System.ServiceModel.Channels;
using Nelibur.ServiceModel.Services.Maps;

namespace Nelibur.ServiceModel.Services
{
    public sealed class SoapServiceProcessor : ServiceProcessor
    {
        public static void Process(Message message)
        {
            RequestMetadata requestMetaData = _requests.FromMessage(message);
            IRequestProcessor context = _requestProcessors.Get(requestMetaData.Type);
            context.Process(requestMetaData);
        }

        public static Message ProcessWithResponse(Message message)
        {
            RequestMetadata requestMetaData = _requests.FromMessage(message);
            IRequestProcessor context = _requestProcessors.Get(requestMetaData.Type);
            return context.ProcessWithResponse(requestMetaData);
        }
    }
}
