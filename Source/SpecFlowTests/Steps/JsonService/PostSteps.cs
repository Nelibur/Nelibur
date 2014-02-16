﻿using System.Linq;
using Nelibur.ServiceModel.Clients;
using SpecFlowTests.Samples.JsonService;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using Xunit;

namespace SpecFlowTests.Steps.JsonService
{
    [Scope(Feature = "Post actions")]
    [Binding]
    public sealed class PostSteps : JsonServiceActionStep
    {
        [When(@"I send data thru PostAsync action")]
        public void WhenISendDataThruPostAsyncAction(Table table)
        {
            OrderJson order = table.CreateSet<OrderJson>().Single();
            JsonServiceClient client = GetClient();
            client.PostAsync(order).Wait();
        }

        [When(@"I send data thru PostAsync with response action")]
        public void WhenISendDataThruPostAsyncWithResponseAction(Table table)
        {
            OrderJson order = table.CreateSet<OrderJson>().Single();
            JsonServiceClient client = GetClient();
            bool response = client.PostAsync<OrderJson, bool>(order).Result;
            ScenarioContext.Current[ResopnseKey] = response;
        }

        [When(@"I send data thru Post with response action")]
        public void WhenISendDataThruPostWithResponseAction(Table table)
        {
            OrderJson order = table.CreateSet<OrderJson>().Single();
            JsonServiceClient client = GetClient();
            bool response = client.Post<OrderJson, bool>(order);
            ScenarioContext.Current[ResopnseKey] = response;
        }

        [When(@"response equals '(.*)'")]
        public void WhenResponseEquals(bool response)
        {
            var actualResponse = (bool)ScenarioContext.Current[ResopnseKey];
            Assert.Equal(response, actualResponse);
        }
    }
}
