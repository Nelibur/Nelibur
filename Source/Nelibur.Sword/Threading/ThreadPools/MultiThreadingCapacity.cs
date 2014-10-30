using System;

namespace Nelibur.Sword.Threading.ThreadPools
{
    /// <summary>
    ///     Represent threading capacity
    /// </summary>
    public enum MultiThreadingCapacity
    {
        /// <summary>
        ///     Represent all processors
        /// </summary>
        Global,

        /// <summary>
        ///     Represent one processor
        /// </summary>
        PerProcessor
    }
}
