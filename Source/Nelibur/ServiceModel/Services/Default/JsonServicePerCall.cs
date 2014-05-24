using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Nelibur.ServiceModel.Contracts;

namespace Nelibur.ServiceModel.Services.Default
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public sealed class JsonServicePerCall : IJsonService
    {
        public Message Delete(Message message)
        {
            return NeliburRestService.Process(message);
        }

        public void DeleteOneWay(Message message)
        {
            NeliburRestService.ProcessOneWay(message);
        }

        public Message Get(Message message)
        {
            return NeliburRestService.Process(message);
        }

        public void GetOneWay(Message message)
        {
            NeliburRestService.ProcessOneWay(message);
        }

        public Message Post(Message message)
        {
            return NeliburRestService.Process(message);
        }

        public void PostOneWay(Message message)
        {
            NeliburRestService.ProcessOneWay(message);
        }

        public Message Put(Message message)
        {
            return NeliburRestService.Process(message);
        }

        public void PutOneWay(Message message)
        {
            NeliburRestService.ProcessOneWay(message);
        }
    }
}
