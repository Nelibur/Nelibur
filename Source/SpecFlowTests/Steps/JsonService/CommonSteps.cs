using System;
using System.Linq;
using System.ServiceModel.Web;
using Nelibur.ServiceModel.Clients;
using Nelibur.ServiceModel.Services;
using Nelibur.ServiceModel.Services.Processors;
using SpecFlowTests.Properties;
using SpecFlowTests.Samples.JsonService;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace SpecFlowTests.Steps.JsonService
{
    [Binding]
    public sealed class CommonSteps : JsonServiceActionStep
    {
        private readonly Settings _settings = Settings.Default;
        private WebServiceHost _jsonService;

        [Given("the Json service was started")]
        public void StartJsonService()
        {
            BindRequestToProcessors();
            var address = new Uri(_settings.JsonServiceAddress);
            _jsonService = new WebServiceHost(typeof(JsonServicePerCall), address);
            _jsonService.Open();
        }

        [Then("the Json service was stopped")]
        public void StopJsonService()
        {
            _jsonService.Close();
        }

        [Given(@"I sent data thru Post action")]
        [When(@"I send data thru Post action")]
        public void WhenISendDataThruPostAction(Table table)
        {
            OrderJson order = table.CreateSet<OrderJson>().Single();
            JsonServiceClient client = GetClient();
            client.Post(order);
        }

        private static void BindRequestToProcessors()
        {
            new RestServiceProcessor()
                .Bind<OrderJson, JsonServiceProcessor>()
                .Bind<GetOrderJsonById, JsonServiceProcessor>();
        }
    }
}
