using System.ServiceModel;
using System.ServiceModel.Channels;
using Nelibur.ServiceModel.Contracts;

namespace Nelibur.ServiceModel.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public sealed class SoapService : ISoapService
    {
        public void Process(Message message)
        {
            SoapServiceProcessor.Process(message);
        }

        public Message ProcessWithResponse(Message message)
        {
            return SoapServiceProcessor.ProcessWithResponse(message);
        }
    }
}
