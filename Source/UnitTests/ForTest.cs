using System;
using System.Reflection;
using System.Reflection.Emit;
using Xunit;

namespace UnitTests
{
    public sealed class ForTest
    {
        private delegate object ObjectActivator();

        [Fact]
        public void Test()
        {
        }

        private static object CreateInstase(Type type)
        {
            ConstructorInfo emptyConstructor = GetEmptyConstructor(type);
            var dynamicMethod = new DynamicMethod("CreateInstance", type, Type.EmptyTypes, true);
            ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
            ilGenerator.Emit(OpCodes.Nop);
            ilGenerator.Emit(OpCodes.Newobj, emptyConstructor);
            ilGenerator.Emit(OpCodes.Ret);
            var activator = (ObjectActivator)dynamicMethod.CreateDelegate(typeof(ObjectActivator));
            return activator();
        }

        private static ConstructorInfo GetEmptyConstructor(Type type)
        {
            return type.GetConstructor(Type.EmptyTypes);
        }
    }
}
