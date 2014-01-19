using System.ServiceModel.Channels;
using Nelibur.ServiceModel.Services.Maps;

namespace Nelibur.ServiceModel.Services.Processors
{
    public sealed class SoapServiceProcessor : ServiceProcessor
    {
        public static void Process(Message message)
        {
            RequestMetadata requestMetaData = _requests.FromSoapMessage(message);
            IRequestProcessor processor = _requestProcessors.Get(requestMetaData.Type);
            processor.Process(requestMetaData);
        }

        public static Message ProcessWithResponse(Message message)
        {
            RequestMetadata requestMetaData = _requests.FromSoapMessage(message);
            IRequestProcessor processor = _requestProcessors.Get(requestMetaData.Type);
            return processor.ProcessWithResponse(requestMetaData);
        }
    }
}
