using System;
using System.Diagnostics;
using Nelibur.ServiceModel.Clients;
using Performance.Nelibur.Contracts;

namespace Performance.Nelibur.Client
{
    public sealed class PerformanceTests
    {
        private const string ServiceAddress = "http://localhost:9095/performance";
        private const int TenK = 10000;

        public void Run(int iterations)
        {
            for (int i = 0; i < iterations; i++)
            {
                Console.WriteLine("\nIteration: {0}\n", i);

                PostOneWay_SingleClient(TenK);
                PutOneWay_SingleClient(TenK);
                DeleteOneWay_SingleClient(TenK);

                Console.WriteLine(Environment.NewLine);

                Get_SingleClient(TenK);
                Post_SingleClient(TenK);
                Put_SingleClient(TenK);
                Delete_SingleClient(TenK);
            }
        }

        private void DeleteOneWay_SingleClient(int batchSize)
        {
            var timer = new Stopwatch();
            timer.Start();
            using (var client = new JsonServiceClient(ServiceAddress))
            {
                for (int i = 0; i < batchSize; i++)
                {
                    client.Delete(new DataRequest());
                }
            }
            timer.Stop();
            Console.WriteLine("{0} took {1} ms, BatchSize: {2}", "DeleteOneWay_SingleClient", timer.ElapsedMilliseconds, batchSize);
        }

        private void Delete_SingleClient(int batchSize)
        {
            var timer = new Stopwatch();
            timer.Start();
            using (var client = new JsonServiceClient(ServiceAddress))
            {
                for (int i = 0; i < batchSize; i++)
                {
                    client.Delete<DataResponse>(new DataRequest());
                }
            }
            timer.Stop();
            Console.WriteLine("{0} took {1} ms, BatchSize: {2}", "Delete_SingleClient", timer.ElapsedMilliseconds, batchSize);
        }

        private void Get_SingleClient(int batchSize)
        {
            var timer = new Stopwatch();
            timer.Start();
            using (var client = new JsonServiceClient(ServiceAddress))
            {
                for (int i = 0; i < batchSize; i++)
                {
                    client.Get<DataResponse>(new DataRequest());
                }
            }
            timer.Stop();
            Console.WriteLine("{0} took {1} ms, BatchSize: {2}", "Get_SingleClient", timer.ElapsedMilliseconds, batchSize);
        }

        private void PostOneWay_SingleClient(int batchSize)
        {
            var timer = new Stopwatch();
            timer.Start();
            using (var client = new JsonServiceClient(ServiceAddress))
            {
                for (int i = 0; i < batchSize; i++)
                {
                    client.Post(new DataRequest());
                }
            }
            timer.Stop();
            Console.WriteLine("{0} took {1} ms, BatchSize: {2}", "PostOneWay_SingleClient", timer.ElapsedMilliseconds, batchSize);
        }

        private void Post_SingleClient(int batchSize)
        {
            var timer = new Stopwatch();
            timer.Start();
            using (var client = new JsonServiceClient(ServiceAddress))
            {
                for (int i = 0; i < batchSize; i++)
                {
                    client.Post<DataResponse>(new DataRequest());
                }
            }
            timer.Stop();
            Console.WriteLine("{0} took {1} ms, BatchSize: {2}", "Post_SingleClient", timer.ElapsedMilliseconds, batchSize);
        }

        private void PutOneWay_SingleClient(int batchSize)
        {
            var timer = new Stopwatch();
            timer.Start();
            using (var client = new JsonServiceClient(ServiceAddress))
            {
                for (int i = 0; i < batchSize; i++)
                {
                    client.Put(new DataRequest());
                }
            }
            timer.Stop();
            Console.WriteLine("{0} took {1} ms, BatchSize: {2}", "PutOneWay_SingleClient", timer.ElapsedMilliseconds, batchSize);
        }

        private void Put_SingleClient(int batchSize)
        {
            var timer = new Stopwatch();
            timer.Start();
            using (var client = new JsonServiceClient(ServiceAddress))
            {
                for (int i = 0; i < batchSize; i++)
                {
                    client.Put<DataResponse>(new DataRequest());
                }
            }
            timer.Stop();
            Console.WriteLine("{0} took {1} ms, BatchSize: {2}", "Put_SingleClient", timer.ElapsedMilliseconds, batchSize);
        }
    }
}
