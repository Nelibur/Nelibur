using System;
using Nelibur.Sword.Threading.ThreadPools.TaskItems;

namespace Nelibur.Sword.Threading.ThreadPools.TaskQueueControllers
{
    public interface ITaskQueueController : IDisposable
    {
        /// <summary>
        /// Gets the consumers waiting.
        /// </summary>
        /// <value>
        /// The consumers waiting.
        /// </value>
        int ConsumersWaiting { get; }

        /// <summary>
        /// Dequeues <see cref="IWorkItem"/>.
        /// </summary>
        /// <returns></returns>
        IWorkItem Dequeue();

        /// <summary>
        /// Enqueues <see cref="IWorkItem"/>.
        /// </summary>
        /// <param name="item">The item.</param>
        void Enqueue(IWorkItem item);

        /// <summary>
        ///     Gets a value indicating whether this instance has tasks.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance has tasks; otherwise, <c>false</c>.
        /// </value>
        bool HasTasks { get; }
    }
}
