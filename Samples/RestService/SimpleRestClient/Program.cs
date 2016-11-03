using System;
using System.Diagnostics;
using System.IO;
using Nelibur.ServiceModel.Clients;
using Nelibur.Sword.Extensions;
using NLog;
using SimpleRestClient.Properties;
using SimpleRestContracts.Contracts;

namespace SimpleRestClient
{
    internal class Program
    {
        private static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;
            //                        PerformanceTest();

            var client = new JsonServiceClient(Settings.Default.ServiceAddress);

            var createRequest = new CreateClientRequest
            {
                Email = "email@email.com"
            };
            var response = client.Post<ClientResponse>(createRequest);
            Console.WriteLine("POST Response: {0}\n", response);

            var updateRequest = new UpdateClientRequest
            {
                Email = "new@email.com",
                Id = response.Id
            };
            response = client.Put<ClientResponse>(updateRequest);
            Console.WriteLine("PUT Response: {0}\n", response);

            var getClientRequest = new GetClientRequest
            {
                Id = response.Id,
                Date = DateTime.Now.Date
            };
            response = client.Get<ClientResponse>(getClientRequest);
            Console.WriteLine("GET Response: {0}\n", response);

            var deleteRequest = new DeleteClientRequest
            {
                Id = response.Id
            };
            client.Delete(deleteRequest);

            var certificate = new MemoryStream(File.ReadAllBytes("Certificate.cer"));
            client.Post(certificate);

            Console.ReadKey();
        }


        private static void PerformanceTest()
        {
            var client = new JsonServiceClient(Settings.Default.ServiceAddress);

            var createRequest = new CreateClientRequest
            {
                Email = "email@email.com"
            };

            Stopwatch stopwatch = Stopwatch.StartNew();

            1000.Times()
                .Iter(x => client.Post<ClientResponse>(createRequest));

            Console.WriteLine("Total: {0} ms", stopwatch.Elapsed.TotalMilliseconds);
        }

        private static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            Logger logger = LogManager.GetCurrentClassLogger();
            var exception = e.ExceptionObject as Exception;

            if (exception == null)
            {
                logger.Error("Unhandled non-CLR exception occured ({0})", e.ExceptionObject);
            }
            else
            {
                logger.Error(
                    "Domain unhandled exception of type {0} occured ({1})",
                    e.GetType().Name,
                    e.ExceptionObject);
            }
        }
    }
}
