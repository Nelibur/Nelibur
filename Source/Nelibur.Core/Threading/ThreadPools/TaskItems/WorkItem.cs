using System;

namespace Nelibur.Core.Threading.ThreadPools.TaskItems
{
    internal sealed class WorkItem : IWorkItem
    {
        private readonly ITaskItem _taskItem;

        private WorkItem(ITaskItem taskItem, TaskItemPriority priority = TaskItemPriority.Normal)
        {
            if (taskItem == null)
            {
                throw new ArgumentNullException("taskItem");
            }
            _taskItem = taskItem;
            Priority = priority;
        }

        public TaskItemPriority Priority { get; private set; }

        public static IWorkItem FromAction(Action action, TaskItemPriority priority = TaskItemPriority.Normal)
        {
            return new ActionTaskItem(action, priority);
        }

        public static IWorkItem FromTaskItem(ITaskItem taskItem, TaskItemPriority priority = TaskItemPriority.Normal)
        {
            return new WorkItem(taskItem, priority);
        }

        public void DoWork()
        {
            _taskItem.DoWork();
        }

        private sealed class ActionTaskItem : IWorkItem
        {
            private readonly Action _action;

            public ActionTaskItem(Action action, TaskItemPriority priority = TaskItemPriority.Normal)
            {
                if (action == null)
                {
                    throw new ArgumentNullException("action");
                }
                _action = action;
                Priority = priority;
            }

            public TaskItemPriority Priority { get; private set; }

            public void DoWork()
            {
                _action();
            }
        }
    }
}
