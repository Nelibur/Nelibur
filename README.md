Nelibur - Message based web service framework on the pure WCF
=======

Nelibur is message based web service framework on the pure [**WCF**](http://msdn.microsoft.com/en-us/library/ms731082.aspx). Nelibur simplifies creating high-performance and message based web services and you certainly have **all the power of the WCF**

Use the [Google Group](https://groups.google.com/forum/#!forum/nelibur) for feature requests and
follow [@Nelibur](http://twitter.com/Nelibur) for project updates.

![Nelibur on Nuget](https://raw.github.com/Nelibur/Nelibur/master/.nuget/NeliburNuget.png)

What to read
============

 * [Building RESTful Message Based Web Services with WCF](http://www.codeproject.com/Articles/712689/Building-RESTful-Message-Based-Web-Services-with-W)
 * [Building SOAP Message Based Web Services with WCF](http://www.codeproject.com/Articles/598157/Message-based-web-service-in-WCF)
 * [How to create REST message based Servcie on the pure WCF](https://github.com/Nelibur/Nelibur/wiki/How-to-create-REST-message-based-Servcie-on-pure-WCF)
 * [How to use REST message based Client](https://github.com/Nelibur/Nelibur/wiki/How-to-use-REST-message-based-Client)
 * [How to create SOAP message based Servcie on the pure WCF](https://github.com/Nelibur/Nelibur/wiki/How-to-create-message-based-web-servcie-in-WCF)
 * [How to use SOAP message based Client](https://github.com/Nelibur/Nelibur/wiki/Wcf-client-for-message-based-web-service)
 
Simple RESTful Message based Client
==================================

```csharp
var client = new JsonServiceClient("http://localhost:8080/webhost");

var createRequest = new CreateClientRequest
    {
        Email = "email@email.com"
    };
ClientResponse response = client.Post<ClientResponse>(createRequest);

var updateRequest = new UpdateClientRequest
    {
        Email = "new@email.com",
        Id = response.Id
    };
response = client.Put<ClientResponse>(updateRequest);

var getClientRequest = new GetClientRequest
    {
        Id = response.Id
    };
response = client.Get<ClientResponse>(getClientRequest);

var deleteRequest = new DeleteClientRequest
    {
        Id = response.Id
    };
client.Delete(deleteRequest);
```	 
 
Simple RESTful Message based service on the pure WCF
===================

The **IJsonService** is flexible, stable and maintainable, we can transfer **any** data, because the service contract depends only from WCF's Message class. "The [Message](http://msdn.microsoft.com/en-us/library/system.servicemodel.channels.message.aspx) class is fundamental to Windows Communication Foundation (WCF). All communication between clients and services ultimately results in Message instances being sent and received." ([MSDN](http://msdn.microsoft.com/en-us/library/ms734675.aspx))

```csharp
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
```

WCF's RESTful service

Nelibur already contains `JsonServicePerCall`, but you can create your own custom Service, for instance

```csharp
[ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
public sealed class SampleWebService : IJsonService
{
    public void DeleteOneWay(Message message)
    {
        NeliburRestService.ProcessOneWay(message);
    }

    public Message Delete(Message message)
    {
        return NeliburRestService.Process(message);
    }

    public void GetOneWay(Message message)
    {
        NeliburRestService.ProcessOneWay(message);
    }

    public Message Get(Message message)
    {
        return NeliburRestService.Process(message);
    }

    public void PostOneWay(Message message)
    {
        NeliburRestService.ProcessOneWay(message);
    }

    public Message Post(Message message)
    {
        return NeliburRestService.Process(message);
    }

    public void PutOneWay(Message message)
    {
        NeliburRestService.ProcessOneWay(message);
    }

    public Message Put(Message message)
    {
        return NeliburRestService.Process(message);
    }
}
```

Request binding on appropriate Processor

```csharp
NeliburRestService.Configure(x =>
{
    x.Bind<CreateClientRequest, ClientProcessor>();
    x.Bind<UpdateClientRequest, ClientProcessor>();
    x.Bind<DeleteClientRequest, ClientProcessor>();
    x.Bind<GetClientRequest, ClientProcessor>();
});
```

ClientProcessor example

```csharp
public sealed class ClientProcessor : IPost<CreateClientRequest>,
									 IGet<GetClientRequest>,
									 IDeleteOneWay<DeleteClientRequest>,
									 IPut<UpdateClientRequest>
{
    private static List<Client> _clients = new List<Client>();

    public void DeleteOneWay(DeleteClientRequest request)
    {
        _clients = _clients.Where(x => x.Id != request.Id).ToList();
    }

    public object Get(GetClientRequest request)
    {
        Client client = _clients.Single(x => x.Id == request.Id);
        return new ClientResponse { Id = client.Id, Email = client.Email };
    }

    public object Post(CreateClientRequest request)
    {
        var client = new Client
        {
            Id = Guid.NewGuid(),
            Email = request.Email
        };
        _clients.Add(client);
        return new ClientResponse { Id = client.Id, Email = client.Email };
    }

    public object Put(UpdateClientRequest request)
    {
        Client client = _clients.Single(x => x.Id == request.Id);
        client.Email = request.Email;
        return new ClientResponse { Id = client.Id, Email = client.Email };
    }
}
```

Simple SOAP Message based Client
==================================

```csharp
var client = new SoapServiceClient("NeliburSoapService");

var createRequest = new CreateClientRequest
    {
        Email = "email@email.com"
    };
ClientResponse response = client.Post<ClientResponse>(createRequest);

var updateRequest = new UpdateClientRequest
    {
        Email = "new@email.com",
        Id = response.Id
    };
response = client.Put<ClientResponse>(updateRequest);

var getClientRequest = new GetClientRequest
    {
        Id = response.Id
    };
response = client.Get<ClientResponse>(getClientRequest);

var deleteRequest = new DeleteClientRequest
    {
        Id = response.Id
    };
client.Delete(deleteRequest);
```
 
Simple SOAP Message based service on the pure WCF
===================
WCF's ServiceContract

```csharp
[ServiceContract]
public interface ISoapService
{

    [OperationContract(Action = SoapServiceMetadata.Action.Process,
        ReplyAction = SoapServiceMetadata.Action.ProcessResponse)]
    Message Process(Message message);

    [OperationContract(Action = SoapServiceMetadata.Action.ProcessOneWay)]
    void ProcessOneWay(Message message);
}
```
	
WCF's SOAP service

```csharp
[ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
public sealed class SampleSoapService : ISoapService
{
    public Message Process(Message message)
    {
        return NeliburSoapService.Process(message);
    }

    public void ProcessOneWay(Message message)
    {
        NeliburSoapService.ProcessOneWay(message);
    }
}
```

Request binding on appropriate Processor

```csharp
NeliburSoapService.Configure(x =>
{
    x.Bind<CreateClientRequest, ClientProcessor>();
    x.Bind<UpdateClientRequest, ClientProcessor>();
    x.Bind<DeleteClientRequest, ClientProcessor>();
    x.Bind<GetClientRequest, ClientProcessor>();
});
```
	
ClientProcessor is the same as for RESTful

```csharp
public sealed class ClientProcessor : IPost<CreateClientRequest>,
									 IGet<GetClientRequest>,
									 IDeleteOneWay<DeleteClientRequest>,
									 IPut<UpdateClientRequest>
{
    private static List<Client> _clients = new List<Client>();

    public void DeleteOneWay(DeleteClientRequest request)
    {
        _clients = _clients.Where(x => x.Id != request.Id).ToList();
    }

    public object Get(GetClientRequest request)
    {
        Client client = _clients.Single(x => x.Id == request.Id);
        return new ClientResponse { Id = client.Id, Email = client.Email };
    }

    public object Post(CreateClientRequest request)
    {
        var client = new Client
        {
            Id = Guid.NewGuid(),
            Email = request.Email
        };
        _clients.Add(client);
        return new ClientResponse { Id = client.Id, Email = client.Email };
    }

    public object Put(UpdateClientRequest request)
    {
        Client client = _clients.Single(x => x.Id == request.Id);
        client.Email = request.Email;
        return new ClientResponse { Id = client.Id, Email = client.Email };
    }
}
```	

Contributing
===================

### Before you do anything else

 * Before reporting an issue or creating a pull request, please discuss it in the [Google Group](https://groups.google.com/group/nelibur)

### To contribute please follow these guidelines:

* Fork the project
* Spaces, not Tabs and no regions
* Make a branch for each thing you want to do
* Send a pull request
