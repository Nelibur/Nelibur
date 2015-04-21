using System;
using System.Configuration;
using Nelibur.Sword.Threading.ThreadPools;
using Nelibur.Sword.Threading.ThreadPools.TaskQueueControllers;
using Xunit;
using Xunit.Extensions;

namespace UnitTests.Nelibur.Sword.Threading.ThreadPools
{
    public sealed class TinyThreadPoolTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Create_InvalidName_ThrowException(string name)
        {
            Assert.Throws<ConfigurationErrorsException>(() => TinyThreadPool.Create(x => { x.Name = name; }));
        }

        [Fact]
        public void Create_InvalidTaskQueueController_ThrowException()
        {
            Assert.Throws<ConfigurationErrorsException>(() => TinyThreadPool.Create(x => { x.TaskQueueController = null; }));
        }

        [Fact]
        public void Create_InvalidTaskQueue_ThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => TinyThreadPool.Create(x => { x.TaskQueueController = new DefaultTaskQueueController(null); }));
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(-1, 1)]
        [InlineData(2, 1)]
        [InlineData(0, 0)]
        public void Create_InvalidThreadRange_ThrowException(int minThreads, int maxThreads)
        {
            Assert.Throws<ConfigurationErrorsException>(() => TinyThreadPool.Create(x =>
            {
                x.MinThreads = minThreads;
                x.MaxThreads = maxThreads;
            }));
        }

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
            threadPool.Dispose();
            threadPool.Dispose();
        }
    }
}
