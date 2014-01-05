using System;
using System.ServiceModel.Web;
using Nelibur.ServiceModel.Services;
using SimpleRestContracts.Contracts;

namespace SimpleRestService
{
    internal class Program
    {
        private static WebServiceHost _service;

        private static void BindRequestToProcessors()
        {
            new RestServcieProcessor()
                .Bind<CreateClientRequest, ClientProcessor>()
                .Bind<UpdateClientRequest, ClientProcessor>()
                .Bind<DeleteClientRequest, ClientProcessor>()
                .Bind<GetClientRequest, ClientProcessor>();
        }

        private static void Main()
        {
            BindRequestToProcessors();
            _service = new WebServiceHost(typeof(SampleWebService));
            _service.Open();
            Console.WriteLine("SampleWebService is running");
            Console.WriteLine("Press any key to exit\n");
            Console.ReadKey();
            _service.Close();
        }
    }
}
