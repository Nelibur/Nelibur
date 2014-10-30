using System;
using System.Collections.Generic;
using Nelibur.Sword.Threading.ThreadPools.TaskItems;

namespace Nelibur.Sword.Threading.ThreadPools.TaskQueues
{
    /// <summary>
    ///     Represent default task queue, does not validate TaskItemPriority
    /// </summary>
    public sealed class DefaultTaskQueue : ITaskQueue
    {
        private readonly Queue<IWorkItem> _queue = new Queue<IWorkItem>();

        public int Count
        {
            get { return _queue.Count; }
        }

        public IWorkItem Dequeue()
        {
            return _queue.Dequeue();
        }

        public void Enqueue(IWorkItem item)
        {
            _queue.Enqueue(item);
        }
    }
}
