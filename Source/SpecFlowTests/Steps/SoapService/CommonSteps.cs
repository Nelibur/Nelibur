using System.Linq;
using Nelibur.ServiceModel.Clients;
using SpecFlowTests.Samples.Contracts;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using Xunit;

namespace SpecFlowTests.Steps.SoapService
{
    [Scope(Tag = "SoapService")]
    [Binding]
    public sealed class CommonSteps : SoapServiceActionStep
    {
        [Given("the Soap service was started")]
        public void StartJsonService()
        {
        }

        [Then("the Soap service was stopped")]
        public void StopJsonService()
        {
        }

        [Given(@"I sent data thru Post action")]
        [When(@"I send data thru Post action")]
        public void WhenISendDataThruPostAction(Table table)
        {
            Order order = table.CreateSet<Order>().Single();
            SoapServiceClient client = GetClient();
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