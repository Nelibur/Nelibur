using System.ServiceModel;
using System.ServiceModel.Channels;
using Nelibur.ServiceModel.Contracts;
using Nelibur.ServiceModel.Services.Processors;

namespace Nelibur.ServiceModel.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public sealed class PerCallSoapService : ISoapService
    {
        /// <summary>
        ///     Process message without response.
        /// </summary>
        /// <param name="message">Request message.</param>
        public void Process(Message message)
        {
            SoapServiceProcessor.Process(message);
        }

        /// <summary>
        ///     Process message with response.
        /// </summary>
        /// <param name="message">Request message.</param>
        /// <returns>Response message.</returns>
        public Message ProcessWithResponse(Message message)
        {
            return SoapServiceProcessor.ProcessWithResponse(message);
        }
    }
}
