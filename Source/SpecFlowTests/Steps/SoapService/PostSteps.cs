using System.Linq;
using Nelibur.ServiceModel.Clients;
using SpecFlowTests.Samples.Contracts;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace SpecFlowTests.Steps.SoapService
{
    [Scope(Feature = "Post soap actions")]
    [Binding]
    public sealed class PostSteps : SoapServiceActionStep
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

        [When(@"I send data thru PostAsync action")]
        public void WhenISendDataThruPostAsyncAction(Table table)
        {
            Order order = table.CreateSet<Order>().Single();
            SoapServiceClient client = GetClient();
            client.PostAsync(order).Wait();
        }

        [When(@"I send data thru PostAsync with response action")]
        public void WhenISendDataThruPostAsyncWithResponseAction(Table table)
        {
            Order order = table.CreateSet<Order>().Single();
            SoapServiceClient client = GetClient();
            bool response = client.PostAsync<bool>(order).Result;
            ScenarioContext.Current[ResopnseKey] = response;
        }

        [When(@"I send data thru Post with response action")]
        public void WhenISendDataThruPostWithResponseAction(Table table)
        {
            Order order = table.CreateSet<Order>().Single();
            SoapServiceClient client = GetClient();
            bool response = client.Post<bool>(order);
            ScenarioContext.Current[ResopnseKey] = response;
        }
    }
}
