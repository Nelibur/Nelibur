using System.ServiceModel;
using System.ServiceModel.Channels;
using Nelibur.ServiceModel.Contracts;

namespace Nelibur.ServiceModel.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public sealed class SoapService : ISoapService
    {
        public Message Process(Message message)
        {
            return SoapServiceProcessor.Process(message);
        }

        public void ProcessWithoutResonse(Message message)
        {
            SoapServiceProcessor.ProcessWithoutResonse(message);
        }
    }
}
