using System.Linq;
using Nelibur.ServiceModel.Clients;
using SpecFlowTests.Samples.Contracts;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using Xunit;

namespace SpecFlowTests.Steps.JsonService
{
    [Binding]
    public sealed class CommonSteps : JsonServiceActionStep
    {
        [Given("the Json service was started")]
        public void StartJsonService()
        {
        }

        [Then("the Json service was stopped")]
        public void StopJsonService()
        {
        }

        [Given(@"I sent data thru Post action")]
        [When(@"I send data thru Post action")]
        public void WhenISendDataThruPostAction(Table table)
        {
            Order order = table.CreateSet<Order>().Single();
            JsonServiceClient client = GetClient();
            client.Post(order);
        }

        [When(@"response equals '(.*)'")]
        public void WhenResponseEquals(bool response)
        {
            var actualResponse = (bool)ScenarioContext.Current[ResopnseKey];
            Assert.Equal(response, actualResponse);
        }
    }
}
