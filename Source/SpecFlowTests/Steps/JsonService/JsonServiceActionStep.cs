using System;
using System.ServiceModel.Web;
using Nelibur.ServiceModel.Clients;
using Nelibur.ServiceModel.Services;
using Nelibur.ServiceModel.Services.Processors;
using SpecFlowTests.Properties;
using SpecFlowTests.Samples.JsonService;

namespace SpecFlowTests.Steps.JsonService
{
    public abstract class JsonServiceActionStep
    {
        protected const string ResopnseKey = "ResopnseKey";
        private static readonly Settings _settings = Settings.Default;
        private static WebServiceHost _jsonService;

        protected static void StartService()
        {
            BindRequestToProcessors();
            var address = new Uri(_settings.JsonServiceAddress);
            _jsonService = new WebServiceHost(typeof(JsonServicePerCall), address);
            _jsonService.Open();
        }

        protected static void StopService()
        {
            _jsonService.Close();
        }

        protected JsonServiceClient GetClient()
        {
            var client = new JsonServiceClient(_settings.JsonServiceAddress);
            return client;
        }

        private static void BindRequestToProcessors()
        {
            new RestServiceProcessor()
                .Bind<OrderJson, JsonServiceProcessor>()
                .Bind<GetOrderJsonById, JsonServiceProcessor>()
                .Bind<DeleteOrderJsonById, JsonServiceProcessor>()
                .Bind<UpdateOrderJson, JsonServiceProcessor>();
        }
    }
}
