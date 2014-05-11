using System;
using System.Collections.Generic;
using System.Linq;
using Nelibur.ServiceModel.Services.Operations;
using SimpleSoapContracts.Contracts;

namespace SimpleSoapService
{
    public sealed class ClientProcessor : IPost<CreateClientRequest>,
        IGet<GetClientRequest>,
        IDeleteOneWay<DeleteClientRequest>,
        IPut<UpdateClientRequest>
    {
        private static List<Client> _clients = new List<Client>();

        public void DeleteOneWay(DeleteClientRequest request)
        {
            Console.WriteLine("Delete Request: {0}\n", request);
            _clients = _clients.Where(x => x.Id != request.Id).ToList();
        }

        public object Get(GetClientRequest request)
        {
            Console.WriteLine("Get Request: {0}", request);
            Client client = _clients.Single(x => x.Id == request.Id);
            return new ClientResponse { Id = client.Id, Email = client.Email };
        }

        public object Post(CreateClientRequest request)
        {
            Console.WriteLine("Post Request: {0}", request);
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
            Console.WriteLine("Put Request: {0}", request);
            Client client = _clients.Single(x => x.Id == request.Id);
            client.Email = request.Email;
            return new ClientResponse { Id = client.Id, Email = client.Email };
        }
    }
}
