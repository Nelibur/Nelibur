﻿using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using Nelibur.Core.DataStructures;

namespace Nelibur.Core.Reflection
{
    public delegate object ObjectActivator();

    public delegate object PropertyGetter(object target);

    public delegate void PropertySetter(object target, object value);

    public static class DelegateFactory
    {
        private static readonly ISafeDictionary<Type, ObjectActivator> _objectActivators = new SafeDictionary<Type, ObjectActivator>();

        public static ObjectActivator CreateCtor(Type type)
        {
            return _objectActivators.GetOrAdd(type, DoCreateCtor);
        }

        public static PropertyGetter CreatePropertyGetter(PropertyInfo property)
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

        public static PropertySetter CreatePropertySetter(PropertyInfo property)
        {
            ParameterExpression target = Expression.Parameter(typeof(object), "target");
            ParameterExpression valueParameter = Expression.Parameter(typeof(object), "value");
            MemberExpression member = Expression.Property(Expression.Convert(target, property.DeclaringType), property);
            MethodInfo convertTo = typeof(DelegateFactory).GetMethod("ConvertTo", BindingFlags.NonPublic | BindingFlags.Static);
            MethodInfo genericConvertTo = convertTo.MakeGenericMethod(property.PropertyType);
            BinaryExpression assignExpression = Expression.Assign(member, Expression.Call(genericConvertTo, valueParameter));
            Expression<PropertySetter> lambda = Expression.Lambda<PropertySetter>(assignExpression, target, valueParameter);
            return lambda.Compile();
        }

        private static T ConvertTo<T>(object value)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
            return (T)converter.ConvertFrom(value);
        }

        private static ObjectActivator DoCreateCtor(Type type)
        {
            ConstructorInfo emptyConstructor = type.GetConstructor(Type.EmptyTypes);
            var dynamicMethod = new DynamicMethod("CreateInstance", type, Type.EmptyTypes, true);
            ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
            ilGenerator.Emit(OpCodes.Nop);
            ilGenerator.Emit(OpCodes.Newobj, emptyConstructor);
            ilGenerator.Emit(OpCodes.Ret);
            return (ObjectActivator)dynamicMethod.CreateDelegate(typeof(ObjectActivator));
        }
    }
}
