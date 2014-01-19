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
            UriTemplate = RestServiceMetadata.UriTemplate.Delete,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void Delete(Message message);

        [OperationContract]
        [WebInvoke(Method = OperationType.Delete,
            UriTemplate = RestServiceMetadata.UriTemplate.DeleteWithResponse,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Message DeleteWithResponse(Message message);

        [OperationContract]
        [WebGet(UriTemplate = RestServiceMetadata.UriTemplate.Get,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void Get(Message message);

        [OperationContract]
        [WebGet(UriTemplate = RestServiceMetadata.UriTemplate.GetWithResponse,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Message GetWithResponse(Message message);

        [OperationContract]
        [WebInvoke(Method = OperationType.Post,
            UriTemplate = RestServiceMetadata.UriTemplate.Post,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void Post(Message message);

        [OperationContract]
        [WebInvoke(Method = OperationType.Post,
            UriTemplate = RestServiceMetadata.UriTemplate.PostWithResponse,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Message PostWithResponse(Message message);

        [OperationContract]
        [WebInvoke(Method = OperationType.Put,
            UriTemplate = RestServiceMetadata.UriTemplate.Put,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void Put(Message message);

        [OperationContract]
        [WebInvoke(Method = OperationType.Put,
            UriTemplate = RestServiceMetadata.UriTemplate.PutWithResponse,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Message PutWithResponse(Message message);
    }
}
