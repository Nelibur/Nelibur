using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Nelibur.ServiceModel.Contracts
{
    [ServiceContract]
    public interface ISoapService
    {
        [OperationContract(Action = SoapServiceMetadata.Action.Process)]
        void Process(Message message);

        [OperationContract(Action = SoapServiceMetadata.Action.ProcessWithResponse,
            ReplyAction = SoapServiceMetadata.Action.ProcessResponse)]
        Message ProcessWithResponse(Message message);
    }
}
