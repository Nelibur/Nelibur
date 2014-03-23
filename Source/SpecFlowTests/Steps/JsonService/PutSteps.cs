using System.Linq;
using Nelibur.ServiceModel.Clients;
using SpecFlowTests.Samples.Contracts;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace SpecFlowTests.Steps.JsonService
{
    [Scope(Feature = "Put actions")]
    [Binding]
    public sealed class PutSteps : JsonServiceActionStep
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

        [When(@"I update data thru Put action")]
        public void WhenIUpdateDataThruPutAction(Table table)
        {
            UpdateOrder request = table.CreateSet<UpdateOrder>().Single();
            JsonServiceClient client = GetClient();
            client.Put(request);
        }

        [When(@"I update data thru Put action with response")]
        public void WhenIUpdateDataThruPutActionWithResponse(Table table)
        {
            UpdateOrder request = table.CreateSet<UpdateOrder>().Single();
            JsonServiceClient client = GetClient();
            bool response = client.Put<UpdateOrder, bool>(request);
            ScenarioContext.Current[ResopnseKey] = response;
        }

        [When(@"I update data thru PutAsync action")]
        public void WhenIUpdateDataThruPutAsyncAction(Table table)
        {
            UpdateOrder request = table.CreateSet<UpdateOrder>().Single();
            JsonServiceClient client = GetClient();
            client.PutAsync(request).Wait();
        }

        [When(@"I update data thru PutAsync action with response")]
        public void WhenIUpdateDataThruPutAsyncActionWithResponse(Table table)
        {
            UpdateOrder request = table.CreateSet<UpdateOrder>().Single();
            JsonServiceClient client = GetClient();
            bool response = client.PutAsync<UpdateOrder, bool>(request).Result;
            ScenarioContext.Current[ResopnseKey] = response;
        }
    }
}
