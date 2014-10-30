using System;
using System.Threading;
using Nelibur.Sword.Threading.ThreadPools.TaskItems;
using Nelibur.Sword.Threading.ThreadPools.TaskQueues;

namespace Nelibur.Sword.Threading.ThreadPools.TaskQueueControllers
{
    public sealed class DefaultTaskQueueController : TaskQueueController
    {
        public DefaultTaskQueueController(ITaskQueue taskQueue)
            : base(taskQueue)
        {
        }

        protected override IWorkItem DequeueCore()
        {
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
                return _taskQueue.Dequeue();
            }
        }

        protected override void EnqueueCore(IWorkItem item)
        {
            lock (_locker)
            {
                _taskQueue.Enqueue(item);
                if (_consumersWaiting > 0)
                {
                    Monitor.PulseAll(_locker);
                }
            }
        }
    }
}
