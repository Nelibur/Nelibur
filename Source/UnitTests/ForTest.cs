using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using Xunit;

namespace UnitTests
{
    public sealed class ForTest
    {
        private delegate object ObjectActivator();

        private delegate object PropertyGetter(object target);

        private delegate void PropertySetter(object target, string value);

        [Fact]
        public void Test()
        {
            //            CheckPropertySetter();
            //            CheckPropertyGetter();
        }

        private static T ConvertTo<T>(string value)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
            return (T)converter.ConvertFrom(value);
        }

        private static ObjectActivator CreateInstase(Type type)
        {
            ConstructorInfo emptyConstructor = GetEmptyConstructor(type);
            var dynamicMethod = new DynamicMethod("CreateInstance", type, Type.EmptyTypes, true);
            ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
            ilGenerator.Emit(OpCodes.Nop);
            ilGenerator.Emit(OpCodes.Newobj, emptyConstructor);
            ilGenerator.Emit(OpCodes.Ret);
            return (ObjectActivator)dynamicMethod.CreateDelegate(typeof(ObjectActivator));
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
            PropertyInfo propertyInfo = typeof(Dummy).GetTypeInfo().GetProperty("ValType");
            PropertySetter propertySetter = CreatePropertySetter(propertyInfo);
            propertySetter(dummy, "1");
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
            ParameterExpression target = Expression.Parameter(typeof(object), "target");
            ParameterExpression valueParameter = Expression.Parameter(typeof(string), "value");
            MemberExpression member = Expression.Property(Expression.Convert(target, property.DeclaringType), property);
            MethodInfo convertTo = typeof(ForTest).GetMethod("ConvertTo", BindingFlags.NonPublic | BindingFlags.Static);
            MethodInfo genericConvertTo = convertTo.MakeGenericMethod(property.PropertyType);
            BinaryExpression assignExpression = Expression.Assign(member, Expression.Call(genericConvertTo, valueParameter));
            Expression<PropertySetter> lambda = Expression.Lambda<PropertySetter>(assignExpression, target, valueParameter);
            return lambda.Compile();
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
