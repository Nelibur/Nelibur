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

	[ServiceContract]
	public interface ISoapService
	{
		[OperationContract(Action = ServiceMetadata.Operations.Process,
			ReplyAction = ServiceMetadata.Operations.ProcessResonse)]
		Message Process(Message message);

		[OperationContract(Action = ServiceMetadata.Operations.ProcessWithoutResult)]
		void ProcessWithoutResult(Message message);
	}

WCF's soap service

	[ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
	public sealed class TestService : ISoapService
	{
		public Message Process(Message message)
		{
			return SoapServiceProcessor.Process(message);
		}

		public void ProcessWithoutResult(Message message)
		{
			SoapServiceProcessor.ProcessWithoutResult(message);
		}
	}

Request binding on appropriate Processor

	SoapServiceProcessor.Bind<CreateClientRequest, ClientProcessor>();
	SoapServiceProcessor.Bind<UpdateClientRequest, ClientProcessor>();
	SoapServiceProcessor.Bind<DeleteClientRequest, ClientProcessor>();
	SoapServiceProcessor.Bind<GetClientRequest, ClientProcessor>();
	
Processor example

    public sealed class ClientProcessor : IPost<CreateClientRequest>, IGet<GetClientRequest>,
                                          IDeleteOneWay<DeleteClientRequest>, IPut<UpdateClientRequest>
    {
        private static List<Client> _clients = new List<Client>();

        public void DeleteOneWay(DeleteClientRequest request)
        {
            _clients = _clients.Where(x => x.Id != request.Id).ToList();
        }

        public object Get(GetClientRequest request)
        {
            Client client = _clients.Single(x => x.Id == request.Id);
            return new ClientResponse {Id = client.Id, Email = client.Email};
        }

        public object Post(CreateClientRequest request)
        {
            var client = new Client
                {
                    Id = Guid.NewGuid(),
                    Email = request.Email
                };
            _clients.Add(client);
            return new ClientResponse {Id = client.Id, Email = client.Email};
        }

        public object Put(UpdateClientRequest request)
        {
            Client client = _clients.Single(x => x.Id == request.Id);
            client.Email = request.Email;
            return new ClientResponse {Id = client.Id, Email = client.Email};
        }
    }

Sample Message based Client in WCF
==================================

	var createRequest = new CreateClientRequest
		{
			Email = "emial@emial.com"
		};
	var response = SoapServiceClient.Post<CreateClientRequest, ClientResponse>(createRequest);

	var updateRequest = new UpdateClientRequest
		{
			Email = "new@email.com",
			Id = response.Id
		};
	response = SoapServiceClient.Put<UpdateClientRequest, ClientResponse>(updateRequest);

	var getClient = new GetClientRequest
		{
			Id = response.Id
		};
	response = SoapServiceClient.Get<GetClientRequest, ClientResponse>(getClient);

	var deleteRequest = new DeleteClientRequest
		{
			Id = response.Id
		};
	SoapServiceClient.Delete(deleteRequest);
	