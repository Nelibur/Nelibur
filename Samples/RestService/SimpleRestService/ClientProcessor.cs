using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Web;
using Nelibur.ServiceModel.Services.Operations;
using SimpleRestContracts.Contracts;

namespace SimpleRestService
{
    public sealed class ClientProcessor : IPost<CreateClientRequest>,
        IGet<GetClientRequest>,
        IDeleteOneWay<DeleteClientRequest>,
        IPut<UpdateClientRequest>,
        IGet<GetCertificateById>
    {
        private static List<Client> _clients = new List<Client>();

        public void DeleteOneWay(DeleteClientRequest request)
        {
            Console.WriteLine("Delete Request: {0}\n", request);
            _clients = _clients.Where(x => x.Id != request.Id).ToList();
        }

        public object Get(GetCertificateById request)
        {
            Console.WriteLine("Id: {0}", request.Id);

            string fileName = Uri.EscapeDataString(string.Format("Certificate-{0}.cer", request.Id));
            string value = string.Format("attachment; filename={0}", fileName);
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Content-Disposition", value);
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Content-Type", "application/x-x509-ca-cert");

            var result = new MemoryStream(File.ReadAllBytes("Certificate.cer"));
            return result;
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


        private sealed class Client
        {
            public string Email { get; set; }
            public Guid Id { get; set; }
        }
    }
}
