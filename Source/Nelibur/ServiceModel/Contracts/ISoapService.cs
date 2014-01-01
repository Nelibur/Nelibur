using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Nelibur.ServiceModel.Contracts
{
    [ServiceContract]
    public interface ISoapService
    {
        [OperationContract(Action = ServiceMetadata.Operations.Process)]
        void Process(Message message);

        [OperationContract(Action = ServiceMetadata.Operations.ProcessWithResponse,
            ReplyAction = ServiceMetadata.Operations.ProcessResponse)]
        Message ProcessWithResponse(Message message);
    }
}
