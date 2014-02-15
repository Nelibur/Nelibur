using System;
using System.ServiceModel.Web;
using Nelibur.ServiceModel.Services;
using Nelibur.ServiceModel.Services.Processors;
using SpecFlowTests.Properties;
using SpecFlowTests.Samples.JsonService;
using TechTalk.SpecFlow;

namespace SpecFlowTests.Steps.JsonService
{
    [Binding]
    public sealed class CommonSteps
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

        private static void BindRequestToProcessors()
        {
            new RestServiceProcessor()
                .Bind<CreateOrderJson, JsonServiceProcessor>();
        }
    }
}
