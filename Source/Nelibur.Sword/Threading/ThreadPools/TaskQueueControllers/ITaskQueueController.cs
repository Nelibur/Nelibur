using System;
using Nelibur.Sword.Threading.ThreadPools.TaskItems;

namespace Nelibur.Sword.Threading.ThreadPools.TaskQueueControllers
{
    public interface ITaskQueueController : IDisposable
    {
        int ConsumersWaiting { get; }
        IWorkItem Dequeue();
        void Enqueue(IWorkItem item);
    }
}
