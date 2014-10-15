using System;
using System.ServiceModel.Web;
using Nelibur.ServiceModel.Services;
using Nelibur.ServiceModel.Services.Default;
using Nelibur.ServiceModel.Services.Operations;
using Performance.Nelibur.Contracts;

namespace Performance.Nelibur.Service
{
    internal class Program
    {
        private static WebServiceHost _service;

        private static void ConfigureService()
        {
            NeliburRestService.Configure(x => x.Bind<DataRequest, DataItemProcessor>());
        }

        private static void Main()
        {
            ConfigureService();
            _service = new WebServiceHost(typeof(JsonServicePerCall));
            _service.Open();
            Console.WriteLine("REST Service is running");
            Console.WriteLine("Press any key to exit\n");
            Console.ReadKey();
            _service.Close();
        }

        private sealed class DataItemProcessor : IPostOneWay<DataRequest>,
            IPost<DataRequest>,
            IPut<DataRequest>,
            IPutOneWay<DataRequest>
        {
            public object Post(DataRequest request)
            {
                return new DataResponse();
            }

            public void PostOneWay(DataRequest request)
            {
            }

            public object Put(DataRequest request)
            {
                return new DataResponse();
            }

            public void PutOneWay(DataRequest request)
            {
            }
        }
    }
}
