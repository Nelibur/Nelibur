using System;
using System.Threading;
using Nelibur.Sword.Threading.ThreadPools.TaskItems;
using Nelibur.Sword.Threading.ThreadPools.TaskQueues;

namespace Nelibur.Sword.Threading.ThreadPools.TaskQueueControllers
{
    public abstract class TaskQueueController : ITaskQueueController
    {
        protected readonly object _locker = new object();
        protected readonly ITaskQueue _taskQueue;
        protected int _consumersWaiting;
        protected bool _isDisposed;

        protected TaskQueueController(ITaskQueue taskQueue)
        {
            if (taskQueue == null)
            {
                throw new ArgumentNullException("taskQueue");
            }
            _taskQueue = taskQueue;
        }

        public virtual int ConsumersWaiting
        {
            get
            {
                lock (_locker)
                {
                    return _consumersWaiting;
                }
            }
        }

        public virtual void Dispose()
        {
            lock (_locker)
            {
                if (_isDisposed)
                {
                    return;
                }
                if (_consumersWaiting > 0)
                {
                    GC.SuppressFinalize(this);
                    _isDisposed = true;
                    Monitor.PulseAll(_locker);
                }
            }
        }

        public IWorkItem Dequeue()
        {
            return DequeueCore();
        }

        public void Enqueue(IWorkItem item)
        {
            EnqueueCore(item);
        }

        protected abstract IWorkItem DequeueCore();
        protected abstract void EnqueueCore(IWorkItem item);
    }
}
