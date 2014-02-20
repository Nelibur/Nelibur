using System.Collections.Generic;
using System.Linq;
using Nelibur.ServiceModel.Clients;
using SpecFlowTests.Samples.JsonService;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using Xunit;

namespace SpecFlowTests.Steps.JsonService
{
    [Scope(Feature = "Get actions")]
    [Binding]
    public sealed class GetSteps : JsonServiceActionStep
    {
        [AfterFeature]
        public static void AfterFeature()
        {
            StopService();
        }

        [BeforeFeature]
        public static void BeforeFeature()
        {
            StartService();
        }

        [Then(@"I get data")]
        public void ThenIGetData(Table table)
        {
            OrderJson expected = table.CreateSet<OrderJson>().Single();
            var actual = (List<OrderJson>)ScenarioContext.Current[ResopnseKey];
            Assert.Equal(1, actual.Count);
            Assert.True(expected.Equals(actual.Single()));
        }

        [Then(@"I request data thru Get action")]
        public void ThenIRequestDataThruGetAction(Table table)
        {
            OrderJson expected = table.CreateSet<OrderJson>().Single();
            var actual = (List<OrderJson>)ScenarioContext.Current[ResopnseKey];
            Assert.Equal(1, actual.Count);
            Assert.True(expected.Equals(actual.Single()));
        }

        [When(@"I request data by Id '(.*)' thru GetAsync action")]
        public void WhenIRequestDataByIdThruGetAsyncAction(int id)
        {
            var request = new GetOrderJsonById
                {
                    Id = id
                };
            JsonServiceClient client = GetClient();
            List<OrderJson> response = client.GetAsync<GetOrderJsonById, List<OrderJson>>(request).Result;
            ScenarioContext.Current[ResopnseKey] = response;
        }

        [When(@"I request data by Id '(.*)' thru Get action")]
        public void WhenIRequestDataByThruIdGetAction(int id)
        {
            var request = new GetOrderJsonById
                {
                    Id = id
                };
            JsonServiceClient client = GetClient();
            List<OrderJson> response = client.Get<GetOrderJsonById, List<OrderJson>>(request);
            ScenarioContext.Current[ResopnseKey] = response;
        }
    }
}
