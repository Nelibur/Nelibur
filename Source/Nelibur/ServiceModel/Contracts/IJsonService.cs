using System;
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
            UriTemplate = RestServiceMetadata.Path.Delete,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Message Delete(Message message);

        [OperationContract]
        [WebInvoke(Method = OperationType.Delete,
            UriTemplate = RestServiceMetadata.Path.DeleteOneWay,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void DeleteOneWay(Message message);

        [OperationContract]
        [WebGet(UriTemplate = RestServiceMetadata.Path.Get,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Message Get(Message message);

        [OperationContract]
        [WebGet(UriTemplate = RestServiceMetadata.Path.GetOneWay,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void GetOneWay(Message message);

        [OperationContract]
        [WebInvoke(Method = OperationType.Post,
            UriTemplate = RestServiceMetadata.Path.Post,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Message Post(Message message);

        [OperationContract]
        [WebInvoke(Method = OperationType.Post,
            UriTemplate = RestServiceMetadata.Path.PostOneWay,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void PostOneWay(Message message);

        [OperationContract]
        [WebInvoke(Method = OperationType.Put,
            UriTemplate = RestServiceMetadata.Path.Put,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Message Put(Message message);

        [OperationContract]
        [WebInvoke(Method = OperationType.Put,
            UriTemplate = RestServiceMetadata.Path.PutOneWay,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void PutOneWay(Message message);
    }
}
