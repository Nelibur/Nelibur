﻿using System.Linq;
using Nelibur.ServiceModel.Clients;
using SpecFlowTests.Samples.JsonService;
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
            UpdateOrderJson request = table.CreateSet<UpdateOrderJson>().Single();
            JsonServiceClient client = GetClient();
            client.Put(request);
        }

        [When(@"I update data thru PutAsync action")]
        public void WhenIUpdateDataThruPutAsyncAction(Table table)
        {
            UpdateOrderJson request = table.CreateSet<UpdateOrderJson>().Single();
            JsonServiceClient client = GetClient();
            client.PutAsync(request).Wait();
        }
    }
}
