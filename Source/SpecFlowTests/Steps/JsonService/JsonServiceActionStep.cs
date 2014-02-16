using Nelibur.ServiceModel.Clients;
using SpecFlowTests.Properties;

namespace SpecFlowTests.Steps.JsonService
{
    public abstract class JsonServiceActionStep
    {
        private readonly Settings _settings = Settings.Default;
        protected const string ResopnseKey = "ResopnseKey";

        protected JsonServiceClient GetClient()
        {
            var client = new JsonServiceClient(_settings.JsonServiceAddress);
            return client;
        }
    }
}
