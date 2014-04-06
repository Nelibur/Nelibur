using System;

namespace Nelibur.Core.Threading.ThreadPools.TaskItems
{
    public interface IWorkItem : ITaskItem
    {
        TaskItemPriority Priority { get; }
    }
}
