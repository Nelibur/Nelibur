using System;
using Nelibur.ServiceModel.Clients;
using SimpleSoapContracts.Contracts;

namespace SimpleSoapClient
{
    internal static class Program
    {
        private static void Main()
        {
            var client = new SoapServiceClient("NeliburSoapService");

            var createRequest = new CreateClientRequest
                {
                    Email = "email@email.com"
                };
            Console.WriteLine("POST Request: {0}", createRequest);
            ClientResponse response = client.Post<CreateClientRequest, ClientResponse>(createRequest);
            Console.WriteLine("POST Response: {0}\n", response);

            var updateRequest = new UpdateClientRequest
                {
                    Email = "new@email.com",
                    Id = response.Id
                };

            Console.WriteLine("PUT Request: {0}", updateRequest);
            response = client.Put<UpdateClientRequest, ClientResponse>(updateRequest);
            Console.WriteLine("PUT Response: {0}\n", response);

            var getClientRequest = new GetClientRequest
                {
                    Id = response.Id
                };
            Console.WriteLine("GET Request: {0}", getClientRequest);
            response = client.Get<GetClientRequest, ClientResponse>(getClientRequest);
            Console.WriteLine("GET Response: {0}\n", response);

            var deleteRequest = new DeleteClientRequest
                {
                    Id = response.Id
                };
            Console.WriteLine("DELETE Request: {0}", deleteRequest);
            client.Delete(deleteRequest);

            Console.ReadKey();
        }
    }
}
