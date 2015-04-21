using System;
using Nelibur.Sword.Core;
using Nelibur.Sword.Threading.ThreadPools.TaskQueueControllers;

namespace Nelibur.Sword.Threading.ThreadPools.Configs
{
    /// <summary>
    ///     ThreadPool config.
    /// </summary>
    public interface IThreadPoolConfig : IFluent
    {
        /// <summary>
        ///     MaxThreads per processor.
        /// </summary>
        /// <remarks>Default value is 5.</remarks>
        int MaxThreads { get; set; }

        /// <summary>
        ///     MinThreads per processor.
        /// </summary>
        /// <remarks>Default value is 1.</remarks>
        int MinThreads { get; set; }

        /// <summary>
        ///     Represents threading capacity
        /// </summary>
        /// <remarks> Default value is PerProcessor.</remarks>
        MultiThreadingCapacity MultiThreadingCapacity { get; set; }

        /// <summary>
        ///     Thread pool name.
        /// </summary>
        /// <remarks>Default value is TinyThreadPool.</remarks>
        string Name { get; set; }

        /// <summary>
        ///     Controls access to the task queue.
        /// </summary>
        /// <remarks>
        ///     Default value is <see cref="DefaultTaskQueueController" />.
        /// </remarks>
        ITaskQueueController TaskQueueController { get; set; }
    }
}
