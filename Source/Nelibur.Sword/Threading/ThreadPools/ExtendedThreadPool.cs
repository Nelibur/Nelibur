using System;
using System.Collections.Generic;
using System.Configuration;
using Nelibur.Sword.Core;
using Nelibur.Sword.Extensions;
using Nelibur.Sword.Threading.ThreadPools.TaskItems;
using Nelibur.Sword.Threading.ThreadPools.TaskQueueControllers;
using Nelibur.Sword.Threading.ThreadPools.TaskQueues;

namespace Nelibur.Sword.Threading.ThreadPools
{
    public sealed class ExtendedThreadPool : IExtendedThreadPool
    {
        private readonly object _locker = new object();
        private readonly ITaskQueueController _taskQueueController;
        private List<WorkThread> _workThreads = new List<WorkThread>();

        private ExtendedThreadPool(Builder builder)
        {
            Name = builder.Name;
            SetThreadingRange(builder);
            MultiThreadingCapacity = builder.MultiThreadingCapacity;
            _taskQueueController = builder.TaskQueueController;
        }

        /// <summary>
        ///     MaxThreads per processor.
        /// </summary>
        public int MaxThreads { get; private set; }

        /// <summary>
        ///     MinThreads per processor.
        /// </summary>
        public int MinThreads { get; private set; }

        /// <summary>
        ///     Represents threading capacity
        /// </summary>
        public MultiThreadingCapacity MultiThreadingCapacity { get; private set; }

        /// <summary>
        ///     Thread pool name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        ///     Add new task.
        /// </summary>
        /// <param name="taskItem">Represent task.</param>
        /// <param name="priority">Task priority.</param>
        /// <remarks>
        ///     Default task priority is <see cref="TaskItemPriority.Normal" />.
        /// </remarks>
        public void AddTask(ITaskItem taskItem, TaskItemPriority priority = TaskItemPriority.Normal)
        {
            IWorkItem workItem = WorkItem.FromTaskItem(taskItem, priority);
            AddWorkItem(workItem);
        }

        /// <summary>
        ///     Add new task as <see cref="Action" />.
        /// </summary>
        /// <param name="action">Represent task.</param>
        /// <param name="priority">Task priority.</param>
        /// <remarks>
        ///     Default task priority is <see cref="TaskItemPriority.Normal" />.
        /// </remarks>
        public void AddTask(Action action, TaskItemPriority priority = TaskItemPriority.Normal)
        {
            IWorkItem workItem = WorkItem.FromAction(action, priority);
            AddWorkItem(workItem);
        }

        /// <summary>
        ///     Stop the ThreadPool.
        /// </summary>
        public void Stop()
        {
            lock (_locker)
            {
                foreach (WorkThread workThread in _workThreads)
                {
                    if (workThread == null)
                    {
                        continue;
                    }
                    workThread.Stop();
                }
                _workThreads = new List<WorkThread>();
            }
        }

        private void AddWorkItem(IWorkItem workItem)
        {
            _taskQueueController.Enqueue(workItem);
            StartNewWorkThreadIfRequire();
        }

        private bool IsWorkThreadRequire()
        {
            if (_workThreads.Count < MaxThreads)
            {
                return _taskQueueController.ConsumersWaiting == 0;
            }
            return false;
        }

        private void SetThreadingRange(Builder builder)
        {
            switch (builder.MultiThreadingCapacity)
            {
                case MultiThreadingCapacity.Global:
                    MinThreads = builder.MinThreads;
                    MaxThreads = builder.MaxThreads;
                    break;
                case MultiThreadingCapacity.PerProcessor:
                    int processorCount = Environment.ProcessorCount;
                    MinThreads = processorCount * builder.MinThreads;
                    MaxThreads = processorCount * builder.MaxThreads;
                    break;
                default:
                    string error = string.Format(
                        "Invalid MultiThreadingCapacity: {0}", builder.MultiThreadingCapacity);
                    throw new ArgumentOutOfRangeException(error);
            }
        }

        private void Start()
        {
            MinThreads.Times(StartNewWorkThread);
        }

        private void StartNewWorkThread()
        {
            WorkThread workThread = new WorkThread.Builder
            {
                Name = string.Format("ExtendedThreadPool WorkThread: {0}", _workThreads.Count),
                TaskQueueController = _taskQueueController
            }.Build();
            _workThreads.Add(workThread);
            workThread.Start();
        }

        private void StartNewWorkThreadIfRequire()
        {
            if (!IsWorkThreadRequire())
            {
                return;
            }
            lock (_locker)
            {
                if (!IsWorkThreadRequire())
                {
                    return;
                }
                StartNewWorkThread();
            }
        }

        public sealed class Builder : IFluent
        {
            public Builder()
            {
                Name = "ExtendedThreadPool";
                MinThreads = 1;
                MaxThreads = 25;
                MultiThreadingCapacity = MultiThreadingCapacity.PerProcessor;
                TaskQueueController = new DefaultTaskQueueController(new DefaultTaskQueue());
            }

            /// <summary>
            ///     MaxThreads per processor.
            /// </summary>
            /// <remarks>Default value is 25.</remarks>
            public int MaxThreads { get; set; }

            /// <summary>
            ///     MinThreads per processor.
            /// </summary>
            /// <remarks>Default value is 1.</remarks>
            public int MinThreads { get; set; }

            /// <summary>
            ///     Represents threading capacity
            /// </summary>
            /// <remarks> Default value is PerProcessor.</remarks>
            public MultiThreadingCapacity MultiThreadingCapacity { get; set; }

            /// <summary>
            ///     Thread pool name.
            /// </summary>
            /// <remarks>Default value is ExtendedThreadPool.</remarks>
            public string Name { get; set; }

            /// <summary>
            ///     Controls access to the task queue.
            /// </summary>
            /// <remarks>
            ///     Default value is <see cref="DefaultTaskQueueController" />.
            /// </remarks>
            public ITaskQueueController TaskQueueController { get; set; }

            /// <summary>
            ///     Creates new instance of <see cref="ExtendedThreadPool" />.
            /// </summary>
            /// <returns>Thread pool instance.</returns>
            public IExtendedThreadPool Build()
            {
                Validate();
                var threadPool = new ExtendedThreadPool(this);
                threadPool.Start();
                return threadPool;
            }

            private void Validate()
            {
                if (TaskQueueController == null)
                {
                    throw new ConfigurationErrorsException("TaskQueueController is null");
                }
                if (string.IsNullOrWhiteSpace(Name))
                {
                    throw new ConfigurationErrorsException("Name is null or white space");
                }
                ValidateThreadingRange();
            }

            private void ValidateThreadingRange()
            {
                if (MinThreads <= 0)
                {
                    string error = string.Format("MinThreads {0} should not be greater zero", MinThreads);
                    throw new ArgumentException(error);
                }
                if (MinThreads > MaxThreads)
                {
                    string error = string.Format("MinThreads {0} should be less MaxThreads {1}", MinThreads, MaxThreads);
                    throw new ArgumentException(error);
                }
            }
        }
    }
}
