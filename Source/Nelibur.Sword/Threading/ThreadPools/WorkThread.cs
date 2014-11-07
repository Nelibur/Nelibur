using System;
using System.Configuration;
using System.Threading;
using Nelibur.Sword.Logging;
using Nelibur.Sword.Threading.ThreadPools.TaskItems;
using Nelibur.Sword.Threading.ThreadPools.TaskQueueControllers;

namespace Nelibur.Sword.Threading.ThreadPools
{
    internal sealed class WorkThread
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(WorkThread));
        private readonly ITaskQueueController _taskQueueController;
        private readonly Thread _thread;
        private volatile bool _isRun = true;

        private WorkThread(Builder builder)
        {
            _taskQueueController = builder.TaskQueueController;
            _thread = new Thread(DoWork)
            {
                Name = builder.Name,
                IsBackground = true,
            };
        }

        public void Start()
        {
            _thread.Start();
        }

        public void Stop()
        {
            _isRun = false;
            _taskQueueController.Dispose();
            _thread.Join();
        }

        private static void ProcessItem(ITaskItem item)
        {
            item.DoWork();
        }

        private void DoWork()
        {
            while (_isRun)
            {
                try
                {
                    IWorkItem workItem = _taskQueueController.Dequeue();
                    if (workItem == null)
                    {
                        continue;
                    }
                    ProcessItem(workItem);
                }
                catch (Exception ex)
                {
                    _log.Error(ex);
                }
            }
        }

        internal sealed class Builder
        {
            public string Name { get; set; }
            public ITaskQueueController TaskQueueController { get; set; }

            public WorkThread Build()
            {
                Validate();
                return new WorkThread(this);
            }

            private void Validate()
            {
                if (TaskQueueController == null)
                {
                    throw new NullReferenceException();
                }
                if (string.IsNullOrWhiteSpace(Name))
                {
                    throw new ConfigurationErrorsException("WorkThread Name is null or whitespace");
                }
            }
        }
    }
}
