using System;

namespace Nelibur.Sword.Threading.ThreadPools.TaskItems
{
    public interface IWorkItem : ITaskItem
    {
        TaskItemPriority Priority { get; }
    }
}
