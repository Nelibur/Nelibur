using System;
using System.Configuration;
using Nelibur.Sword.Threading.ThreadPools.TaskQueueControllers;
using Nelibur.Sword.Threading.ThreadPools.TaskQueues;

namespace Nelibur.Sword.Threading.ThreadPools.Configs
{
    internal sealed class ThreadPoolConfig : IThreadPoolConfig
    {
        internal ThreadPoolConfig()
        {
            Name = "TinyThreadPool";
            MinThreads = 1;
            MaxThreads = 5;
            MultiThreadingCapacity = MultiThreadingCapacity.PerProcessor;
            TaskQueueController = new DefaultTaskQueueController(new DefaultTaskQueue());
        }

        public int MaxThreads { get; set; }
        public int MinThreads { get; set; }
        public MultiThreadingCapacity MultiThreadingCapacity { get; set; }
        public string Name { get; set; }
        public ITaskQueueController TaskQueueController { get; set; }

        internal void Validate()
        {
            if (TaskQueueController == null)
            {
                throw new ConfigurationErrorsException("TaskQueueController is null");
            }
            if (string.IsNullOrEmpty(Name))
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
                throw new ConfigurationErrorsException(error);
            }
            if (MinThreads > MaxThreads)
            {
                string error = string.Format("MinThreads {0} should be less or equal MaxThreads {1}", MinThreads, MaxThreads);
                throw new ConfigurationErrorsException(error);
            }
        }
    }
}
