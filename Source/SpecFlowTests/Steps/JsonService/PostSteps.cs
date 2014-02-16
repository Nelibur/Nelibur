using System.Linq;
using Nelibur.ServiceModel.Clients;
using SpecFlowTests.Properties;
using SpecFlowTests.Samples.JsonService;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using Xunit;

namespace SpecFlowTests.Steps.JsonService
{
    [Scope(Feature = "Post actions")]
    [Binding]
    public sealed class PostSteps
    {
        private const string PostResopnseKey = "PostResopnseKey";
        private readonly Settings _settings = Settings.Default;

        [When(@"I send data thru Post action")]
        public void WhenISendDataThruPostAction(Table table)
        {
            CreateOrderJson order = table.CreateSet<CreateOrderJson>().Single();
            JsonServiceClient client = GetJsonServiceClient();
            client.Post(order);
        }

        [When(@"I send data thru PostAsync action")]
        public void WhenISendDataThruPostAsyncAction(Table table)
        {
            CreateOrderJson order = table.CreateSet<CreateOrderJson>().Single();
            JsonServiceClient client = GetJsonServiceClient();
            client.PostAsync(order).Wait();
        }

        [When(@"I send data thru PostAsync with response action")]
        public void WhenISendDataThruPostAsyncWithResponseAction(Table table)
        {
            CreateOrderJson order = table.CreateSet<CreateOrderJson>().Single();
            JsonServiceClient client = GetJsonServiceClient();
            bool response = client.PostAsync<CreateOrderJson, bool>(order).Result;
            ScenarioContext.Current[PostResopnseKey] = response;
        }

        [When(@"I send data thru Post with response action")]
        public void WhenISendDataThruPostWithResponseAction(Table table)
        {
            CreateOrderJson order = table.CreateSet<CreateOrderJson>().Single();
            JsonServiceClient client = GetJsonServiceClient();
            bool response = client.Post<CreateOrderJson, bool>(order);
            ScenarioContext.Current[PostResopnseKey] = response;
        }

        [When(@"response equals '(.*)'")]
        public void WhenResponseEquals(bool response)
        {
            var actualResponse = (bool)ScenarioContext.Current[PostResopnseKey];
            Assert.Equal(response, actualResponse);
        }

        private JsonServiceClient GetJsonServiceClient()
        {
            var client = new JsonServiceClient(_settings.JsonServiceAddress);
            return client;
        }
    }
}
