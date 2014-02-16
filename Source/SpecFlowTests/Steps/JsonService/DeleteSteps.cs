using Nelibur.ServiceModel.Clients;
using SpecFlowTests.Samples.JsonService;
using TechTalk.SpecFlow;

namespace SpecFlowTests.Steps.JsonService
{
    [Scope(Feature = "Delete actions")]
    [Binding]
    public sealed class DeleteSteps : JsonServiceActionStep
    {
        [When(@"I send delete request by Id '(.*)' thru Delete action")]
        public void WhenISendDeleteRequestByIdThruDeleteAction(int id)
        {
            var request = new DeleteOrderJsonById
                {
                    Id = id
                };
            JsonServiceClient client = GetClient();
            client.Delete(request);
        }

        [When(@"I send delete request by Id '(.*)' thru DeleteAsync action")]
        public void WhenISendDeleteRequestByIdThruDeleteAsyncAction(int id)
        {
            var request = new DeleteOrderJsonById
                {
                    Id = id
                };
            JsonServiceClient client = GetClient();
            client.DeleteAsync(request).Wait();
        }

        [When(@"I send delete request by Id '(.*)' thru DeleteAsync with response action")]
        public void WhenISendDeleteRequestByIdThruDeleteAsyncWithResponseAction(int id)
        {
            var request = new DeleteOrderJsonById
                {
                    Id = id
                };
            JsonServiceClient client = GetClient();
            bool response = client.DeleteAsync<DeleteOrderJsonById, bool>(request).Result;
            ScenarioContext.Current[ResopnseKey] = response;
        }

        [When(@"I send delete request by Id '(.*)' thru Delete with response action")]
        public void WhenISendDeleteRequestByIdThruDeleteWithResponseAction(int id)
        {
            var request = new DeleteOrderJsonById
                {
                    Id = id
                };
            JsonServiceClient client = GetClient();
            bool response = client.Delete<DeleteOrderJsonById, bool>(request);
            ScenarioContext.Current[ResopnseKey] = response;
        }
    }
}
