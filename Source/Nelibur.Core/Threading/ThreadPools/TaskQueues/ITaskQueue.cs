using System;
using Nelibur.Core.Threading.ThreadPools.TaskItems;

namespace Nelibur.Core.Threading.ThreadPools.TaskQueues
{
    public interface ITaskQueue
    {
        int Count { get; }
        IWorkItem Dequeue();
        void Enqueue(IWorkItem item);
    }
}
