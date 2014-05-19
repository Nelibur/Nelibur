using System.ServiceModel;
using Nelibur.ServiceModel.Clients;
using Nelibur.ServiceModel.Services;
using Nelibur.ServiceModel.Services.Processors;
using SpecFlowTests.Samples.Contracts;
using SpecFlowTests.Samples.SoapService;

namespace SpecFlowTests.Steps.SoapService
{
    public abstract class SoapServiceActionStep
    {
        protected const string ResopnseKey = "ResopnseKey";
        private const string EndpointConfigurationName = "NeliburSoapService";
        private static ServiceHost _serviceHost;

        protected static void StartService()
        {
            BindRequestToProcessors();
            _serviceHost = new ServiceHost(typeof(SoapServicePerCall));
            _serviceHost.Open();
        }

        protected static void StopService()
        {
            _serviceHost.Close();
        }

        protected SoapServiceClient GetClient()
        {
            var client = new SoapServiceClient(EndpointConfigurationName);
            return client;
        }

        private static void BindRequestToProcessors()
        {
            NeliburSoapService.Configure(x =>
            {
                x.Bind<Order, OrderServiceProcessor>();
                x.Bind<GetOrderById, OrderServiceProcessor>();
                x.Bind<DeleteOrderById, OrderServiceProcessor>();
                x.Bind<UpdateOrder, OrderServiceProcessor>();
            });
        }
    }
}
