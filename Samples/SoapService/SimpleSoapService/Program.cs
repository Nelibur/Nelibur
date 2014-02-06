using System;
using System.ServiceModel;
using Nelibur.ServiceModel.Services;
using Nelibur.ServiceModel.Services.Processors;
using SimpleSoapContracts.Contracts;

namespace SimpleSoapService
{
    internal static class Program
    {
        private static ServiceHost _service;

        private static void BindRequestToProcessors()
        {
            new SoapServiceProcessor()
                .Bind<CreateClientRequest, ClientProcessor>()
                .Bind<UpdateClientRequest, ClientProcessor>()
                .Bind<DeleteClientRequest, ClientProcessor>()
                .Bind<GetClientRequest, ClientProcessor>();
        }

        private static void Main()
        {
            BindRequestToProcessors();
            _service = new ServiceHost(typeof(SoapServicePerCall));
            _service.Open();
            Console.WriteLine("SampleSoapService is running");
            Console.WriteLine("Press any key to exit\n");
            Console.ReadKey();
            _service.Close();
        }
    }
}
