using System;
using System.ServiceModel.Web;
using Nelibur.ServiceModel.Services.Processors;
using SimpleRestContracts.Contracts;

namespace SimpleRestService
{
    internal class Program
    {
        private static WebServiceHost _service;

        private static void BindRequestToProcessors()
        {
            new RestServiceProcessor()
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
            Console.WriteLine("Sample REST Service is running");
            Console.WriteLine("Press any key to exit\n");
            Console.ReadKey();
            _service.Close();
        }
    }
}
