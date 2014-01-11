using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Nelibur.ServiceModel.Contracts
{
    [ServiceContract]
    public interface ISoapService
    {
        [OperationContract(Action = SoapServiceMetadata.Operations.Process)]
        void Process(Message message);

        [OperationContract(Action = SoapServiceMetadata.Operations.ProcessWithResponse,
            ReplyAction = SoapServiceMetadata.Operations.ProcessResponse)]
        Message ProcessWithResponse(Message message);
    }
}
