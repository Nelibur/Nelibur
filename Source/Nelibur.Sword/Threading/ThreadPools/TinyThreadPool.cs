using System;
using System.Collections.Generic;
using Nelibur.Sword.Extensions;
using Nelibur.Sword.Threading.ThreadPools.Configs;
using Nelibur.Sword.Threading.ThreadPools.TaskItems;
using Nelibur.Sword.Threading.ThreadPools.TaskQueueControllers;

namespace Nelibur.Sword.Threading.ThreadPools
{
    /// <summary>
    ///     Represents <see cref="ITinyThreadPool" />.
    /// </summary>
    public static class TinyThreadPool
    {
        /// <summary>
        ///     Gets the default <see cref="ITinyThreadPool" /> with default config.
        /// </summary>
        /// <value>
        ///     The default <see cref="ITinyThreadPool" />.
        /// </value>
        /// <remarks>See <see cref="IThreadPoolConfig" /> for default config values.</remarks>
        public static ITinyThreadPool Default
        {
            get
            {
                var threadPool = new ThreadPool(new ThreadPoolConfig());
                threadPool.Start();
                return threadPool;
            }
        }

        /// <summary>
        ///     Creates new instance of the <see cref="ITinyThreadPool" />.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <returns>New instance of the <see cref="ITinyThreadPool" />.</returns>
        public static ITinyThreadPool Create(Action<IThreadPoolConfig> config)
        {
            var threadPoolConfig = new ThreadPoolConfig();
            config(threadPoolConfig);

            var threadPool = new ThreadPool(threadPoolConfig);
            threadPool.Start();
            return threadPool;
        }

        private sealed class ThreadPool : ITinyThreadPool
        {
            private readonly object _locker = new object();
            private readonly ITaskQueueController _taskQueueController;
            private bool _isDisposed;
            private List<WorkThread> _workThreads = new List<WorkThread>();

            internal ThreadPool(ThreadPoolConfig config)
            {
                config.Validate();
                SetThreadingRange(config);
                Name = config.Name;
                MultiThreadingCapacity = config.MultiThreadingCapacity;
                _taskQueueController = config.TaskQueueController;
            }

            public int MaxThreads { get; private set; }
            public int MinThreads { get; private set; }
            public MultiThreadingCapacity MultiThreadingCapacity { get; private set; }

            /// <inheritdoc />
            public bool HasTasks => _taskQueueController.HasTasks;

            public string Name { get; private set; }

            /// <summary>
            ///     Returns a <see cref="System.String" /> that represents this instance.
            /// </summary>
            /// <returns>
            ///     A <see cref="System.String" /> that represents this instance.
            /// </returns>
            public override string ToString()
            {
                return string.Format("ThreadPool Name: {0}, MultiThreadingCapacity: {1}, MinThreads: {2}, MaxThreads: {3}",
                    Name, MultiThreadingCapacity, MinThreads, MaxThreads);
            }

            /// <summary>
            ///     Stops ThreadPool.
            /// </summary>
            public void Dispose()
            {
                lock (_locker)
                {
                    if (_isDisposed)
                    {
                        return;
                    }
                    Stop();
                    _isDisposed = true;
                }
            }

            public void AddTask(ITaskItem taskItem, TaskItemPriority priority = TaskItemPriority.Normal)
            {
                IWorkItem workItem = WorkItem.FromTaskItem(taskItem, priority);
                AddWorkItem(workItem);
            }

            public void AddTask(Action action, TaskItemPriority priority = TaskItemPriority.Normal)
            {
                IWorkItem workItem = WorkItem.FromAction(action, priority);
                AddWorkItem(workItem);
            }

            internal void Start()
            {
                MinThreads.Times(StartNewWorkThread);
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

            private void SetThreadingRange(ThreadPoolConfig config)
            {
                switch (config.MultiThreadingCapacity)
                {
                    case MultiThreadingCapacity.Global:
                        MinThreads = config.MinThreads;
                        MaxThreads = config.MaxThreads;
                        break;
                    case MultiThreadingCapacity.PerProcessor:
                        int processorCount = Environment.ProcessorCount;
                        MinThreads = processorCount * config.MinThreads;
                        MaxThreads = processorCount * config.MaxThreads;
                        break;
                    default:
                        string error = string.Format(
                            "Invalid MultiThreadingCapacity: {0}", config.MultiThreadingCapacity);
                        throw new ArgumentOutOfRangeException(error);
                }
            }

            private void StartNewWorkThread()
            {
                WorkThread workThread = new WorkThread.Builder
                {
                    Name = string.Format("TinyThreadPool WorkThread: {0}", _workThreads.Count),
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

            private void Stop()
            {
                if (_workThreads.IsNullOrEmpty())
                {
                    return;
                }
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
    }
}
