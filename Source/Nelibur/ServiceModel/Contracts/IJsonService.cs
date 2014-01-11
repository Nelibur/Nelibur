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
            UriTemplate = RestServiceMetadata.UriTemplates.Delete,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void Delete(Message message);

        [OperationContract]
        [WebInvoke(Method = OperationType.Delete,
            UriTemplate = RestServiceMetadata.UriTemplates.DeleteWithResponse,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Message DeleteWithResponse(Message message);

        [OperationContract]
        [WebGet(UriTemplate = RestServiceMetadata.UriTemplates.Get,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void Get(Message message);

        [OperationContract]
        [WebGet(UriTemplate = RestServiceMetadata.UriTemplates.GetWithResponse,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Message GetWithResponse(Message message);

        [OperationContract]
        [WebInvoke(Method = OperationType.Post,
            UriTemplate = RestServiceMetadata.UriTemplates.Post,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void Post(Message message);

        [OperationContract]
        [WebInvoke(Method = OperationType.Post,
            UriTemplate = RestServiceMetadata.UriTemplates.PostWithResponse,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Message PostWithResponse(Message message);

        [OperationContract]
        [WebInvoke(Method = OperationType.Put,
            UriTemplate = RestServiceMetadata.UriTemplates.Put,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void Put(Message message);

        [OperationContract]
        [WebInvoke(Method = OperationType.Put,
            UriTemplate = RestServiceMetadata.UriTemplates.PutWithResponse,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Message PutWithResponse(Message message);
    }
}
