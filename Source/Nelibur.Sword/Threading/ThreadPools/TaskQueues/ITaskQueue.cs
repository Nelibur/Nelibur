using System;
using Nelibur.Sword.Threading.ThreadPools.TaskItems;

namespace Nelibur.Sword.Threading.ThreadPools.TaskQueues
{
    public interface ITaskQueue
    {
        int Count { get; }
        IWorkItem Dequeue();
        void Enqueue(IWorkItem item);
    }
}
