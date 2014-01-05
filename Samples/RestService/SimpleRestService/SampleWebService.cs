using System.ServiceModel;
using System.ServiceModel.Channels;
using Nelibur.ServiceModel.Contracts;
using Nelibur.ServiceModel.Services;

namespace SimpleRestService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public sealed class SampleWebService : IRestService
    {
        public void Delete(Message message)
        {
            RestServcieProcessor.Process(message);
        }

        public Message Get(Message message)
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
