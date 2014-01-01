using System;
using Nelibur.Core.Threading.ThreadPools.TaskItems;

namespace Nelibur.Core.Threading.ThreadPools.TaskQueueControllers
{
    public interface ITaskQueueController : IDisposable
    {
        int ConsumersWaiting { get; }
        IWorkItem Dequeue();
        void Enqueue(IWorkItem item);
    }
}
