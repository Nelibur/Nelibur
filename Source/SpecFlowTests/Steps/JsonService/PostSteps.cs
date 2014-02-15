using System.Linq;
using Nelibur.ServiceModel.Clients;
using SpecFlowTests.Properties;
using SpecFlowTests.Samples.JsonService;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace SpecFlowTests.Steps.JsonService
{
    [Scope(Feature = "Post actions")]
    [Binding]
    public sealed class PostSteps
    {
        private readonly Settings _settings = Settings.Default;

        [When(@"I send data thru Post action")]
        public void WhenISendDataThruPostAction(Table table)
        {
            CreateOrderJson order = table.CreateSet<CreateOrderJson>().Single();
            var client = new JsonServiceClient(_settings.JsonServiceAddress);
            client.Post(order);
        }
    }
}
