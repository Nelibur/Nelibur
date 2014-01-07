using System;
using System.Collections.Generic;
using Xunit;

namespace UnitTests
{
    public sealed class ForTest
    {
        [Fact]
        public void Test()
        {
            var container = new Container();
            container.Register(() => new Item());
        }

        private sealed class Container
        {
            private readonly Dictionary<Type, Func<object>> _repository = new Dictionary<Type, Func<object>>();

            public void Register<T>(Func<T> func)
            {
                _repository[typeof(T)] = () => func();
            }
        }

        private sealed class Item
        {
        }
    }
}
