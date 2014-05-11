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

        public void Delete(DeleteClientRequest request)
        {
            Console.WriteLine("Delete Request: {0}\n", request);
            _clients = _clients.Where(x => x.Id != request.Id).ToList();
        }

        public object GetWithResponse(GetClientRequest request)
        {
            Console.WriteLine("GetWithResponse Request: {0}", request);
            Client client = _clients.Single(x => x.Id == request.Id);
            return new ClientResponse { Id = client.Id, Email = client.Email };
        }

        public object PostWithResponse(CreateClientRequest request)
        {
            Console.WriteLine("PostWithResponse Request: {0}", request);
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
            Console.WriteLine("PutWithResponse Request: {0}", request);
            Client client = _clients.Single(x => x.Id == request.Id);
            client.Email = request.Email;
            return new ClientResponse { Id = client.Id, Email = client.Email };
        }
    }
}
