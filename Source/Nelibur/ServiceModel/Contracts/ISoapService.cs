using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Nelibur.ServiceModel.Contracts
{
    [ServiceContract]
    public interface ISoapService
    {
        /// <summary>
        ///     Process message with response.
        /// </summary>
        /// <param name="message">Request message.</param>
        /// <returns>Response message.</returns>
        [OperationContract(Action = SoapServiceMetadata.Action.Process,
            ReplyAction = SoapServiceMetadata.Action.ProcessResponse)]
        Message Process(Message message);

        /// <summary>
        ///     Process message without response.
        /// </summary>
        /// <param name="message">Request message.</param>
        [OperationContract(Action = SoapServiceMetadata.Action.ProcessOneWay)]
        void ProcessOneWay(Message message);
    }
}
