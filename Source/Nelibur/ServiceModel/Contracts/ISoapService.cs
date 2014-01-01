using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Nelibur.ServiceModel.Contracts
{
    [ServiceContract]
    public interface ISoapService
    {
        [OperationContract(Action = ServiceMetadata.Operations.Process,
            ReplyAction = ServiceMetadata.Operations.ProcessResponse)]
        Message Process(Message message);

        [OperationContract(Action = ServiceMetadata.Operations.ProcessWithoutResponse)]
        void ProcessWithoutResonse(Message message);
    }
}
