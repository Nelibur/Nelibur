using System;
using System.ServiceModel.Web;
using Nelibur.ServiceModel.Services;
using Nelibur.ServiceModel.Services.Default;
using SimpleRestContracts.Contracts;

namespace SimpleRestService
{
    internal class Program
    {
        private static WebServiceHost _service;

        private static void BindRequestToProcessors()
        {
            NeliburRestService.Configure(x =>
            {
                x.Bind<CreateClientRequest, ClientProcessor>();
                x.Bind<UpdateClientRequest, ClientProcessor>();
                x.Bind<DeleteClientRequest, ClientProcessor>();
                x.Bind<GetClientRequest, ClientProcessor>();
            });
        }

        private static void Main()
        {
            BindRequestToProcessors();
            _service = new WebServiceHost(typeof(JsonServicePerCall));
            _service.Open();
            Console.WriteLine("Sample REST Service is running");
            Console.WriteLine("Press any key to exit\n");
            Console.ReadKey();
            _service.Close();
        }
    }
}
