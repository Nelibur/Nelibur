using System;
using Nelibur.ServiceModel.Clients;
using SpecFlowTests.Samples.Contracts;
using TechTalk.SpecFlow;

namespace SpecFlowTests.Steps.SoapService
{
    [Scope(Feature = "Delete soap actions")]
    [Binding]
    public sealed class DeleteSteps : SoapServiceActionStep
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

        [When(@"I send delete request by Id '(.*)' thru Delete action")]
        public void WhenISendDeleteRequestByIdThruDeleteAction(int id)
        {
            var request = new DeleteOrderById
            {
                Id = id
            };
            using (SoapServiceClient client = GetClient())
            {
                client.Delete(request);
            }
        }

        [When(@"I send delete request by Id '(.*)' thru DeleteAsync action")]
        public void WhenISendDeleteRequestByIdThruDeleteAsyncAction(int id)
        {
            var request = new DeleteOrderById
            {
                Id = id
            };
            using (SoapServiceClient client = GetClient())
            {
                client.DeleteAsync(request).Wait();
            }
        }

        [When(@"I send delete request by Id '(.*)' thru DeleteAsync with response action")]
        public void WhenISendDeleteRequestByIdThruDeleteAsyncWithResponseAction(int id)
        {
            var request = new DeleteOrderById
            {
                Id = id
            };
            SoapServiceClient client = GetClient();
            bool response = client.DeleteAsync<bool>(request).Result;
            ScenarioContext.Current[ResopnseKey] = response;
        }

        [When(@"I send delete request by Id '(.*)' thru Delete with response action")]
        public void WhenISendDeleteRequestByIdThruDeleteWithResponseAction(int id)
        {
            var request = new DeleteOrderById
            {
                Id = id
            };
            SoapServiceClient client = GetClient();
            var response = client.Delete<bool>(request);
            ScenarioContext.Current[ResopnseKey] = response;
        }
    }
}
