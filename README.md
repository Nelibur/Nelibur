Nelibur - Message based webservice framework in WCF
=======

Nelibur is message based webservice framework fully based on WCF. Nelibur simplifies creating high-performance and message based webservices in WCF.

What to read
============

 * [Message based web service in WCF](http://www.codeproject.com/Articles/598157/Message-based-web-service-in-WCF)
 * [Extended Thread Pool](http://www.codeproject.com/Articles/27358/Extended-Thread-Pool)
 * [Dynamic Visitor Pattern](http://www.codeproject.com/Articles/563043/Dynamic-Visitor-Pattern)
 * [Simple Password Generator](http://www.codeproject.com/Tips/428262/Simple-Password-Generator)
 
Sample Message based service in WCF
===================
WCF's ServiceContract

```csharp
[ServiceContract]
public interface ISoapService
{
	[OperationContract(Action = ServiceMetadata.Operations.Process)]
	void Process(Message message);

	[OperationContract(Action = ServiceMetadata.Operations.ProcessWithResponse,
		ReplyAction = ServiceMetadata.Operations.ProcessResponse)]
	Message ProcessWithResponse(Message message);
}
```
	
WCF's soap service

```csharp
[ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
public sealed class SampleSoapService : ISoapService
{
	public void Process(Message message)
	{
		SoapServiceProcessor.Process(message);
	}

	public Message ProcessWithResponse(Message message)
	{
		return SoapServiceProcessor.ProcessWithResponse(message);
	}
}
```

Request binding on appropriate Processor

```csharp
new SoapServiceProcessor()
	.Bind<CreateClientRequest, ClientProcessor>()
	.Bind<UpdateClientRequest, ClientProcessor>()
	.Bind<DeleteClientRequest, ClientProcessor>()
	.Bind<GetClientRequest, ClientProcessor>();
```
	
Processor example

```csharp
public sealed class ClientProcessor : IPostWithResponse<CreateClientRequest>,
									IGetWithResponse<GetClientRequest>,
									IDelete<DeleteClientRequest>,
									IPutWithResponse<UpdateClientRequest>
{
	private static List<Client> _clients = new List<Client>();

	public void Delete(DeleteClientRequest request)
	{
		_clients = _clients.Where(x => x.Id != request.Id).ToList();
	}

	public object GetWithResponse(GetClientRequest request)
	{
		Client client = _clients.Single(x => x.Id == request.Id);
		return new ClientResponse { Id = client.Id, Email = client.Email };
	}

	public object PostWithResponse(CreateClientRequest request)
	{
		var client = new Client
			{
				Id = Guid.NewGuid(),
				Email = request.Email
			};
		_clients.Add(client);
		return new ClientResponse { Id = client.Id, Email = client.Email };
	}

	public object PutWithResponse(UpdateClientRequest request)
	{
		Client client = _clients.Single(x => x.Id == request.Id);
		client.Email = request.Email;
		return new ClientResponse { Id = client.Id, Email = client.Email };
	}
}
```	

Sample Message based Client in WCF
==================================

```csharp
var createRequest = new CreateClientRequest
	{
		Email = "email@email.com"
	};
ClientResponse response = SoapServiceClient.Post<CreateClientRequest, ClientResponse>(createRequest);

var updateRequest = new UpdateClientRequest
	{
		Email = "new@email.com",
		Id = response.Id
	};
response = SoapServiceClient.Put<UpdateClientRequest, ClientResponse>(updateRequest);

var getClientRequest = new GetClientRequest
	{
		Id = response.Id
	};
response = SoapServiceClient.Get<GetClientRequest, ClientResponse>(getClientRequest);

var deleteRequest = new DeleteClientRequest
	{
		Id = response.Id
	};
SoapServiceClient.Delete(deleteRequest);
```	