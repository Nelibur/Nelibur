using System;
using System.Collections.Generic;
using System.Threading;
using Nelibur.Core.Extensions;

namespace Nelibur.Core.DataStructures.Queues
{
    public sealed class BlockingQueue<T> : IDisposable
    {
        private readonly Queue<T> _queue = new Queue<T>();
        private int _consumersWaiting;
        private bool _isDisposed;

        ~BlockingQueue()
        {
            lock (_queue)
            {
                if (!_isDisposed)
                {
                    _isDisposed = true;
                    Monitor.PulseAll(_queue);
                }
            }
        }

        public int Count
        {
            get
            {
                lock (_queue)
                {
                    return _queue.Count;
                }
            }
        }

        public bool IsEmpty
        {
            get
            {
                lock (_queue)
                {
                    return _queue.Count == 0;
                }
            }
        }

        public T Dequeue()
        {
            T value;
            lock (_queue)
            {
                while (_queue.Count == 0)
                {
                    _consumersWaiting++;
                    Monitor.Wait(_queue);
                    _consumersWaiting--;
                }
                value = _queue.Dequeue();
            }
            return value;
        }

        public void Enqueue(T value)
        {
            lock (_queue)
            {
                _queue.Enqueue(value);
                if (_consumersWaiting > 0)
                {
                    Monitor.PulseAll(_queue);
                }
            }
        }

        public void Enqueue(IEnumerable<T> values)
        {
            lock (_queue)
            {
                values.Iter(_queue.Enqueue);
                if (_consumersWaiting > 0)
                {
                    Monitor.PulseAll(_queue);
                }
            }
        }

        public void Dispose()
        {
            lock (_queue)
            {
                if (!_isDisposed)
                {
                    GC.SuppressFinalize(this);
                    _isDisposed = true;
                    Monitor.PulseAll(_queue);
                }
            }
        }
    }
}
