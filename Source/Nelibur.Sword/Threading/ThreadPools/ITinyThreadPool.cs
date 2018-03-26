using System;
using Nelibur.Sword.Threading.ThreadPools.TaskItems;

namespace Nelibur.Sword.Threading.ThreadPools
{
    public interface ITinyThreadPool : IDisposable
    {
        /// <summary>
        ///     MaxThreads per processor.
        /// </summary>
        int MaxThreads { get; }

        /// <summary>
        ///     MinThreads per processor.
        /// </summary>
        int MinThreads { get; }

        /// <summary>
        ///     Represents threading capacity
        /// </summary>
        MultiThreadingCapacity MultiThreadingCapacity { get; }

        /// <summary>
        ///     Gets a value indicating whether this instance has tasks.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance has tasks; otherwise, <c>false</c>.
        /// </value>
        bool HasTasks { get; }

        /// <summary>
        ///     Thread pool name.
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     Add new task.
        /// </summary>
        /// <param name="taskItem">Represent task.</param>
        /// <param name="priority">Task priority.</param>
        /// <remarks>
        ///     Default task priority is <see cref="TaskItemPriority.Normal" />.
        /// </remarks>
        void AddTask(ITaskItem taskItem, TaskItemPriority priority = TaskItemPriority.Normal);

        /// <summary>
        ///     Add new task.
        /// </summary>
        /// <param name="action">Represent task.</param>
        /// <param name="priority">Task priority.</param>
        /// <remarks>
        ///     Default task priority is <see cref="TaskItemPriority.Normal" />.
        /// </remarks>
        void AddTask(Action action, TaskItemPriority priority = TaskItemPriority.Normal);
    }
}
