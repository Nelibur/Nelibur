using System;
using System.Collections.Generic;
using System.Threading;
using Nelibur.Core.DataStructures.Queues;
using Nelibur.Core.Extensions;
using Nelibur.Core.Logging;

namespace Nelibur.Core.Threading.Processors
{
    public abstract class TaskProcessor<TTask> : IDisposable
    {
        private readonly ILog _log;
        private readonly BlockingQueue<TTask> _queue = new BlockingQueue<TTask>();
        private volatile bool _isRun = true;
        private Thread _workThread;

        protected TaskProcessor()
        {
            _log = LogManager.GetLogger(GetType());
            Initialise();
        }

        public int Count
        {
            get { return _queue.Count; }
        }

        /// <summary>
        ///     Processes the task.
        /// </summary>
        /// <param name="task">The task.</param>
        public void Enqueue(TTask task)
        {
            _queue.Enqueue(task);
        }

        /// <summary>
        ///     Processes tasks.
        /// </summary>
        /// <param name="tasks">Tasks.</param>
        public void Enqueue(IEnumerable<TTask> tasks)
        {
            _queue.Enqueue(tasks);
        }

        public void Dispose()
        {
            if (_isRun == false)
            {
                return;
            }
            _isRun = false;
            DisposeCore();
        }

        protected virtual void DisposeCore()
        {
        }

        protected abstract void ProcessTaskCore(TTask task);

        private void Initialise()
        {
            _workThread = new Thread(ProcessTask)
                {
                    Name = GetType().Name,
                    IsBackground = true
                };
            _workThread.Start();
        }

        private void ProcessTask()
        {
            while (_isRun)
            {
                try
                {
                    TTask task = _queue.Dequeue();
                    if (task.IsNull())
                    {
                        continue;
                    }
                    _log.DebugFormat("Processing task: {0}", task);
                    ProcessTaskCore(task);
                }
                catch (Exception ex)
                {
                    _log.Error(ex);
                }
            }
        }
    }
}
