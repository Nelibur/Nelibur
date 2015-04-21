using System;
using Nelibur.Sword.Threading.ThreadPools;
using Xunit;

namespace UnitTests.Nelibur.Sword.Threading.ThreadPools
{
    public sealed class TinyThreadPoolTests
    {
        [Fact]
        public void Create_NewThreadPool_Success()
        {
            const string Name = "MyThreadPool";
            const int MinThreads = 4;
            const int MaxThreads = 6;
            const MultiThreadingCapacity MultiThreadingCapacity = MultiThreadingCapacity.Global;

            ITinyThreadPool threadPool = TinyThreadPool.Create(x =>
            {
                x.Name = Name;
                x.MinThreads = MinThreads;
                x.MaxThreads = MaxThreads;
                x.MultiThreadingCapacity = MultiThreadingCapacity;
            });

            Assert.Equal(MultiThreadingCapacity, threadPool.MultiThreadingCapacity);
            Assert.Equal(MinThreads, threadPool.MinThreads);
            Assert.Equal(MaxThreads, threadPool.MaxThreads);
            Assert.Equal(Name, threadPool.Name);
        }

        [Fact]
        public void Default_NewThreadPool_Success()
        {
            ITinyThreadPool threadPool = TinyThreadPool.Default;
            Assert.Equal(MultiThreadingCapacity.PerProcessor, threadPool.MultiThreadingCapacity);
            Assert.Equal(1, threadPool.MinThreads);
            Assert.Equal(5, threadPool.MaxThreads);
            Assert.Equal("TinyThreadPool", threadPool.Name);
            threadPool.Stop();
        }
    }
}
