using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Nelibur.ServiceModel.Contracts;
using Nelibur.ServiceModel.Services.Processors;

namespace Nelibur.ServiceModel.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public sealed class JsonServicePerCall : IJsonService
    {
        public Message Delete(Message message)
        {
            return RestServiceProcessor.ProcessWithResponse(message);
        }

        public void DeleteOneWay(Message message)
        {
            RestServiceProcessor.Process(message);
        }

        public Message Get(Message message)
        {
            return RestServiceProcessor.ProcessWithResponse(message);
        }

        public void GetOneWay(Message message)
        {
            RestServiceProcessor.Process(message);
        }

        public Message Post(Message message)
        {
            return RestServiceProcessor.ProcessWithResponse(message);
        }

        public void PostOneWay(Message message)
        {
            RestServiceProcessor.Process(message);
        }

        public Message Put(Message message)
        {
            return RestServiceProcessor.ProcessWithResponse(message);
        }

        public void PutOneWay(Message message)
        {
            RestServiceProcessor.Process(message);
        }
    }
}
