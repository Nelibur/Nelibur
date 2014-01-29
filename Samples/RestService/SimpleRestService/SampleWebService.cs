using System.ServiceModel;
using System.ServiceModel.Channels;
using Nelibur.ServiceModel.Contracts;
using Nelibur.ServiceModel.Services.Processors;

namespace SimpleRestService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public sealed class SampleWebService : IJsonService
    {
        public void Delete(Message message)
        {
            RestServiceProcessor.Process(message);
        }

        public Message DeleteWithResponse(Message message)
        {
            return RestServiceProcessor.ProcessWithResponse(message);
        }

        public void Get(Message message)
        {
            RestServiceProcessor.Process(message);
        }

        public Message GetWithResponse(Message message)
        {
            return RestServiceProcessor.ProcessWithResponse(message);
        }

        public void Post(Message message)
        {
            RestServiceProcessor.Process(message);
        }

        public Message PostWithResponse(Message message)
        {
            return RestServiceProcessor.ProcessWithResponse(message);
        }

        public void Put(Message message)
        {
            RestServiceProcessor.Process(message);
        }

        public Message PutWithResponse(Message message)
        {
            return RestServiceProcessor.ProcessWithResponse(message);
        }
    }
}
