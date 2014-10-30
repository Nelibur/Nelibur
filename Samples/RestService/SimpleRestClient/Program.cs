using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Nelibur.ServiceModel.Clients;
using Nelibur.Sword.Extensions;
using NLog;
using SimpleRestClient.Properties;
using SimpleRestContracts.Contracts;

namespace SimpleRestClient
{
    internal class Program
    {
        [Conditional("DEBUG")]
        private static void Demo()
        {
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
        }

        private static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;
            Demo();
            PerformanceTests();

            Console.ReadKey();
        }

        private static void PerformanceTest(int threads, int count)
        {
            var createRequest = new CreateClientPerformanceRequest
            {
                Email = "email@email.com"
            };
            //            var client = new JsonServiceClient(Settings.Default.ServiceAddress);
            Stopwatch stopwatch = Stopwatch.StartNew();

            Parallel.For(0, threads, i =>
            {
                var client = new JsonServiceClient(Settings.Default.ServiceAddress);
                count.Times().Iter(x => client.Post<ClientResponse>(createRequest));
            });

            Console.WriteLine("Threads: {3},\tTimes: {1},\tTotal: {0:#} ms,\t {2:#} m/s",
                stopwatch.Elapsed.TotalMilliseconds,
                count,
                (count * threads) / stopwatch.Elapsed.TotalSeconds,
                threads);
        }

        [Conditional("PERFORMANCE")]
        private static void PerformanceTests()
        {
            //            PerformanceTest(1, 1000);
            //            PerformanceTest(2, 1000);
            //
            //            PerformanceTest(1, 5000);
            //            PerformanceTest(2, 5000);
            //
            //            PerformanceTest(1, 10000);

            //PerformanceTest(1, 100000);
            //PerformanceTest(1, 1000000);

            //            PerformanceTest(1, 10000);
            //            PerformanceTest(2, 10000);
            //            PerformanceTest(4, 10000);
            //            PerformanceTest(8, 10000);
            //            PerformanceTest(16, 10000);

            PerformanceTest(2, 16000);
            PerformanceTest(4, 8000);
            PerformanceTest(8, 4000);
            PerformanceTest(16, 2000);
            PerformanceTest(32, 1000);
            PerformanceTest(64, 500);
            PerformanceTest(128, 250);
            PerformanceTest(256, 125);
            PerformanceTest(512, 64);
            PerformanceTest(1024, 32);
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
