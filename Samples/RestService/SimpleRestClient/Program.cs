using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Nelibur.ServiceModel.Clients;
using SimpleRestContracts.Contracts;

namespace SimpleRestClient
{
    internal class Program
    {
        private static void Main()
        {
            //            PerformanceTest();
            var client = new RestServiceClient("NeliburRestService");

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

        private static void PerformanceTest()
        {
            var client = new RestServiceClient("NeliburRestService");

            var createRequest = new CreateClientRequest
                {
                    Email = "email@email.com"
                };

            Stopwatch stopwatch = Stopwatch.StartNew();

            Task<ClientResponse>[] tasks = Enumerable
                .Range(0, 100000)
                .ToList()
                .Select(x => Task.Run(() => client.Post<CreateClientRequest, ClientResponse>(createRequest))).ToArray();

            Task.WaitAll(tasks);

            Console.WriteLine("Total: {0} ms", stopwatch.Elapsed.TotalMilliseconds);
        }
    }
}
