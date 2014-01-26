using System.ServiceModel;
using System.ServiceModel.Channels;
using Nelibur.ServiceModel.Contracts;
using Nelibur.ServiceModel.Services.Processors;

namespace Nelibur.ServiceModel.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public sealed class JsonServicePerCall : IJsonService
    {
        public void Delete(Message message)
        {
            RestServcieProcessor.Process(message);
        }

        public Message DeleteWithResponse(Message message)
        {
            return RestServcieProcessor.ProcessWithResponse(message);
        }

        public void Get(Message message)
        {
            RestServcieProcessor.Process(message);
        }

        public Message GetWithResponse(Message message)
        {
            return RestServcieProcessor.ProcessWithResponse(message);
        }

        public void Post(Message message)
        {
            RestServcieProcessor.Process(message);
        }

        public Message PostWithResponse(Message message)
        {
            return RestServcieProcessor.ProcessWithResponse(message);
        }

        public void Put(Message message)
        {
            RestServcieProcessor.Process(message);
        }

        public Message PutWithResponse(Message message)
        {
            return RestServcieProcessor.ProcessWithResponse(message);
        }
    }
}
