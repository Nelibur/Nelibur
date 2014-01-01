using System;
using System.Threading;
using Nelibur.Core.Threading.ThreadPools.TaskItems;
using Nelibur.Core.Threading.ThreadPools.TaskQueues;

namespace Nelibur.Core.Threading.ThreadPools.TaskQueueControllers
{
    public sealed class BoundedTaskQueueController : TaskQueueController
    {
        private readonly int _maxTasksCount;
        private int _producersWaiting;

        public BoundedTaskQueueController(ITaskQueue taskQueue, int maxTasksCount)
            : base(taskQueue)
        {
            if (maxTasksCount < 1)
            {
                throw new ArgumentException("MaxTasksCount should be greater 0");
            }
            _maxTasksCount = maxTasksCount;
        }

        protected override IWorkItem DequeueCore()
        {
            IWorkItem taskItem;
            lock (_locker)
            {
                while (_taskQueue.Count == 0 && !_isDisposed)
                {
                    _consumersWaiting++;
                    Monitor.Wait(_locker);
                    _consumersWaiting--;
                }
                if (_isDisposed)
                {
                    return null;
                }
                taskItem = _taskQueue.Dequeue();
                if (_producersWaiting > 0)
                {
                    Monitor.PulseAll(_locker);
                }
            }
            return taskItem;
        }

        protected override void EnqueueCore(IWorkItem item)
        {
            lock (_locker)
            {
                while (_taskQueue.Count == (_maxTasksCount - 1) && !_isDisposed)
                {
                    _producersWaiting++;
                    Monitor.Wait(_locker);
                    _producersWaiting--;
                }
                _taskQueue.Enqueue(item);
                if (_consumersWaiting > 0)
                {
                    Monitor.PulseAll(_locker);
                }
            }
        }
    }
}
