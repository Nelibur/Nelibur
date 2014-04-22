using System;
using System.Text;
using Nelibur.Core.Reflection;
using Xunit;

namespace UnitTests.Nelibur.Core.Reflection
{
    public sealed class DelegateFactoryTest
    {
        [Fact]
        public void CreateCtor_Null_ThrowException()
        {
            Assert.Throws(typeof(NullReferenceException), () => DelegateFactory.CreateCtor(null));
        }

        [Fact]
        public void CreateCtor_StringBuilder_Ok()
        {
            ObjectActivator ctor = DelegateFactory.CreateCtor(typeof(StringBuilder));
            Assert.IsType<StringBuilder>(ctor());
        }

        [Fact]
        public void CreatePropertyGetter_Dummy_Ok()
        {
            PropertyGetter getter = DelegateFactory.CreatePropertyGetter(typeof(Dummy).GetProperty("Id"));
            var dummy = new Dummy { Id = 1 };
            object actual = getter(dummy);
            Assert.Equal(dummy.Id, actual);
        }

        [Fact]
        public void CreatePropertyGetter_Null_ThrowException()
        {
            Assert.Throws(typeof(NullReferenceException), () => DelegateFactory.CreatePropertyGetter(null));
        }

        [Fact]
        public void CreatePropertySetter_Dummy_Ok()
        {
            PropertySetter setter = DelegateFactory.CreatePropertySetter(typeof(Dummy).GetProperty("Id"));
            var dummy = new Dummy();
            setter(dummy, "1");
            Assert.Equal(dummy.Id, 1);
        }

        [Fact]
        public void CreatePropertySetter_Null_ThrowException()
        {
            Assert.Throws(typeof(NullReferenceException), () => DelegateFactory.CreatePropertySetter(null));
        }

        private sealed class Dummy
        {
            public int Id { get; set; }
        }
    }
}
