using System.Collections.Generic;
using System.Linq;
using Nelibur.ServiceModel.Clients;
using SpecFlowTests.Samples.JsonService;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using Xunit;

namespace SpecFlowTests.Steps.JsonService
{
    [Binding]
    public sealed class GetSteps : JsonServiceActionStep
    {
        [Then(@"I get data")]
        public void ThenIGetData(Table table)
        {
            OrderJson expected = table.CreateSet<OrderJson>().Single();
            var actual = (List<OrderJson>)ScenarioContext.Current[ResopnseKey];
            Assert.Equal(1, actual.Count);
            Assert.True(expected.Equals(actual.Single()));
        }

        [When(@"I request data by Id '(.*)'")]
        public void WhenIRequestDataById(int id)
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
