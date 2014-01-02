using System.ServiceModel;
using System.ServiceModel.Channels;
using Nelibur.ServiceModel.Contracts;
using Nelibur.ServiceModel.Services;

namespace SimpleSoapService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public sealed class SampleSoapService : ISoapService
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
