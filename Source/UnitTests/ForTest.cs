using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Xunit;

namespace UnitTests
{
    public sealed class ForTest
    {
        private delegate object ObjectActivator();

        private delegate object PropertyGetter(object target);

        private delegate void PropertySetter(object target, object value);

        [Fact]
        public void Test()
        {
            //            CheckPropertySetter();
            //            CheckPropertyGetter();
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

        private void CheckPropertyGetter()
        {
            var dummy = new Dummy { RefType = "Test" };
            Stopwatch stopwatch = Stopwatch.StartNew();
            PropertyInfo propertyInfo = typeof(Dummy).GetTypeInfo().GetProperty("RefType");
            PropertyGetter propertyGetter = CreatePropertyGetter(propertyInfo);
            Console.WriteLine(propertyGetter(dummy));
            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed.TotalMilliseconds);
        }

        private void CheckPropertySetter()
        {
            var dummy = new Dummy();
            Stopwatch stopwatch = Stopwatch.StartNew();
            PropertyInfo propertyInfo = typeof(Dummy).GetTypeInfo().GetProperty("RefType");
            PropertySetter propertySetter = CreatePropertySetter(propertyInfo);
            propertySetter(dummy, "10");
            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed.TotalMilliseconds);
        }

        private PropertyGetter CreatePropertyGetter(PropertyInfo property)
        {
            var method = new DynamicMethod("Get" + property.Name, typeof(object), new[] { typeof(object) }, true);
            ILGenerator ilGenerator = method.GetILGenerator();
            Type propertyType = property.DeclaringType;
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Castclass, propertyType);
            ilGenerator.Emit(OpCodes.Callvirt, property.GetGetMethod());
            if (property.PropertyType.IsValueType)
            {
                ilGenerator.Emit(OpCodes.Box, property.PropertyType);
            }
            ilGenerator.Emit(OpCodes.Ret);
            return (PropertyGetter)method.CreateDelegate(typeof(PropertyGetter));
        }

        private PropertySetter CreatePropertySetter(PropertyInfo property)
        {
            var method = new DynamicMethod("Set" + property.Name, null, new[] { typeof(object), typeof(object) }, true);
            ILGenerator ilGenerator = method.GetILGenerator();
            Type propertyType = property.DeclaringType;

            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Castclass, propertyType);
            ilGenerator.Emit(OpCodes.Ldarg_1);
            if (property.PropertyType.IsValueType)
            {
                ilGenerator.Emit(OpCodes.Unbox_Any, property.PropertyType);
            }
            else
            {
                ilGenerator.Emit(OpCodes.Castclass, property.PropertyType);
            }
            ilGenerator.Emit(OpCodes.Callvirt, property.GetSetMethod());
            ilGenerator.Emit(OpCodes.Ret);

            return (PropertySetter)method.CreateDelegate(typeof(PropertySetter));
        }

        private void Process<T>(T value)
        {
            List<string> properties = typeof(T).GetTypeInfo()
                .GetProperties()
                .Select(x => x.Name).ToList();
            typeof(T).GetTypeInfo().GetProperties().Select(x => x.GetValue(value));
        }

        private string TestGet(object source)
        {
            return ((Dummy)source).RefType;
        }

        private void TestSet(object source, object value)
        {
            ((Dummy)source).RefType = (string)value;
        }

        private sealed class Dummy
        {
            public string RefType { get; set; }
            public int ValType { get; set; }

            public override string ToString()
            {
                return string.Format("ValType: {0}", ValType);
            }
        }
    }
}
