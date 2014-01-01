using System;
using System.Collections.Generic;
using Nelibur.Core.Threading.ThreadPools.TaskItems;

namespace Nelibur.Core.Threading.ThreadPools.TaskQueues
{
    public sealed class PriorityTaskQueue : ITaskQueue
    {
        private readonly List<Queue<IWorkItem>> _queues = new List<Queue<IWorkItem>>();
        private int _workItemCount;

        public PriorityTaskQueue()
        {
            int capacity = Enum.GetNames(typeof(TaskItemPriority)).Length;
            _queues = new List<Queue<IWorkItem>>(capacity);

            for (int i = 0; i < _queues.Capacity; i++)
            {
                _queues.Add(new Queue<IWorkItem>());
            }
        }

        public int Count
        {
            get { return _workItemCount; }
        }

        public IWorkItem Dequeue()
        {
            for (int i = _queues.Count - 1; i >= 0; i--)
            {
                if (_queues[i].Count == 0)
                {
                    continue;
                }
                IWorkItem workItem = _queues[i].Dequeue();
                _workItemCount--;
                return workItem;
            }
            return null;
        }

        public void Enqueue(IWorkItem item)
        {
            var priority = (int)item.Priority;
            if (priority >= _queues.Count)
            {
                throw new ArgumentException(string.Format("Invalid TaskItemPriority: {0}", item.Priority));
            }
            _queues[priority].Enqueue(item);
            _workItemCount++;
        }
    }
}
