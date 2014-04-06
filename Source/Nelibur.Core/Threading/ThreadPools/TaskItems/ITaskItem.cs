using System;

namespace Nelibur.Core.Threading.ThreadPools.TaskItems
{
    public interface ITaskItem
    {
        void DoWork();
    }
}
