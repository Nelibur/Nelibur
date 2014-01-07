using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using Nelibur.ServiceModel.Services.Operations;

namespace Nelibur.ServiceModel.Contracts
{
    [ServiceContract]
    public interface IJsonService
    {
        [OperationContract]
        [WebInvoke(Method = OperationType.Delete,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void Delete(Message message);

        [OperationContract]
        [WebInvoke(Method = OperationType.Delete,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Message DeleteWithResponse(Message message);

        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void Get(Message message);

        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Message GetWithResponse(Message message);

        [OperationContract]
        [WebInvoke(Method = OperationType.Post,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void Post(Message message);

        [OperationContract]
        [WebInvoke(Method = OperationType.Post,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Message PostWithResponse(Message message);

        [OperationContract]
        [WebInvoke(Method = OperationType.Put,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void Put(Message message);

        [OperationContract]
        [WebInvoke(Method = OperationType.Put,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Message PutWithResponse(Message message);
    }
}
