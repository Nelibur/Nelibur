using System.ServiceModel.Channels;
using Nelibur.ServiceModel.Services.Maps;

namespace Nelibur.ServiceModel.Services.Processors
{
    public sealed class SoapServiceProcessor : ServiceProcessor
    {
        public static void Process(Message message)
        {
            RequestMetadata metadata = _requests.FromSoapMessage(message);
            Process(metadata);
        }

        public static Message ProcessWithResponse(Message message)
        {
            RequestMetadata metadata = _requests.FromSoapMessage(message);
            return ProcessWithResponse(metadata);
        }
    }
}
