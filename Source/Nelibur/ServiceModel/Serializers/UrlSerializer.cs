using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Web;
using Nelibur.Core;
using Nelibur.ServiceModel.Contracts;

namespace Nelibur.ServiceModel.Serializers
{
    internal sealed class UrlSerializer
    {
        private static readonly Dictionary<Type, ObjectCreator> _objectCreators = new Dictionary<Type, ObjectCreator>();

        private UrlSerializer(NameValueCollection value)
        {
            QueryParams = value;
        }

        private delegate object ObjectActivator();

        private delegate object PropertyGetter(object target);

        private delegate void PropertySetter(object target, string value);

        public NameValueCollection QueryParams { get; private set; }

        public static UrlSerializer FromQueryParams(NameValueCollection value)
        {
            if (value == null)
            {
                throw Error.ArgumentNull("value");
            }
            return new UrlSerializer(value);
        }

        public static UrlSerializer FromType(Type value)
        {
            if (value == null)
            {
                throw Error.ArgumentNull("value");
            }
            NameValueCollection collection = CreateQueryParams(value);
            return new UrlSerializer(collection);
        }

        public static UrlSerializer FromValue<T>(T value)
            where T : class
        {
            if (value == null)
            {
                throw Error.ArgumentNull("value");
            }
            NameValueCollection collection = new QueryStringCreator<T>().GetNameValueCollection(value);
            return new UrlSerializer(collection);
        }

        public object GetRequestValue(Type targetType)
        {
            ObjectCreator objectCreator;
            if (!_objectCreators.TryGetValue(targetType, out objectCreator))
            {
                objectCreator = new ObjectCreator(targetType);
                _objectCreators[targetType] = objectCreator;
            }
            return objectCreator.Create(QueryParams);
        }

        public string GetTypeValue()
        {
            const string Key = RestServiceMetadata.ParamName.Type;
            return UrlDecode(QueryParams[Key]);
        }

        private static NameValueCollection CreateQueryParams(Type value)
        {
            return new NameValueCollection
                   {
                       { RestServiceMetadata.ParamName.Type, UrlEncode(value.Name) },
                   };
        }

        private static string UrlDecode(string value)
        {
            return HttpUtility.UrlDecode(value);
        }

        /// <remarks>http://stackoverflow.com/questions/602642/server-urlencode-vs-httputility-urlencode</remarks>
        /// <remarks>http://blogs.msdn.com/b/yangxind/archive/2006/11/09/don-t-use-net-system-uri-unescapedatastring-in-url-decoding.aspx</remarks>
        private static string UrlEncode(string value)
        {
            return Uri.EscapeDataString(value);
        }

        private sealed class ObjectCreator
        {
            private readonly ObjectActivator _objectActivator;
            private readonly Dictionary<string, PropertySetter> _setters;

            public ObjectCreator(Type type)
            {
                _objectActivator = CreateInstase(type);
                _setters = type
                    .GetTypeInfo()
                    .GetProperties()
                    .ToDictionary(x => x.Name, x => CreatePropertySetter(x), StringComparer.OrdinalIgnoreCase);
            }

            public object Create(NameValueCollection collection)
            {
                object result = _objectActivator();
                foreach (string key in collection.AllKeys)
                {
                    if (string.Equals(key, RestServiceMetadata.ParamName.Type, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }
                    string value = UrlDecode(collection[key]);
                    _setters[key](result, value);
                }
                return result;
            }

            private static T ConvertTo<T>(string value)
            {
                TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
                return (T)converter.ConvertFrom(value);
            }

            private static ObjectActivator CreateInstase(Type type)
            {
                ConstructorInfo emptyConstructor = type.GetConstructor(Type.EmptyTypes);
                var dynamicMethod = new DynamicMethod("CreateInstance", type, Type.EmptyTypes, true);
                ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
                ilGenerator.Emit(OpCodes.Nop);
                ilGenerator.Emit(OpCodes.Newobj, emptyConstructor);
                ilGenerator.Emit(OpCodes.Ret);
                return (ObjectActivator)dynamicMethod.CreateDelegate(typeof(ObjectActivator));
            }

            private PropertySetter CreatePropertySetter(PropertyInfo property)
            {
                ParameterExpression target = Expression.Parameter(typeof(object), "target");
                ParameterExpression valueParameter = Expression.Parameter(typeof(string), "value");
                MemberExpression member = Expression.Property(Expression.Convert(target, property.DeclaringType), property);
                MethodInfo convertTo = typeof(ObjectCreator).GetMethod("ConvertTo", BindingFlags.NonPublic | BindingFlags.Static);
                MethodInfo genericConvertTo = convertTo.MakeGenericMethod(property.PropertyType);
                BinaryExpression assignExpression = Expression.Assign(member, Expression.Call(genericConvertTo, valueParameter));
                Expression<PropertySetter> lambda = Expression.Lambda<PropertySetter>(assignExpression, target, valueParameter);
                return lambda.Compile();
            }
        }

        private sealed class QueryStringCreator<T>
            where T : class
        {
            private readonly Dictionary<PropertyInfo, PropertyGetter> _getters;
            private readonly NameValueCollection _typeInfo = CreateQueryParams(typeof(T));

            public QueryStringCreator()
            {
                _getters = typeof(T)
                    .GetTypeInfo()
                    .GetProperties()
                    .ToDictionary(x => x, x => CreatePropertyGetter(x));
            }

            public NameValueCollection GetNameValueCollection(T value)
            {
                var result = new NameValueCollection(_typeInfo);
                foreach (KeyValuePair<PropertyInfo, PropertyGetter> item in _getters)
                {
                    string itemValue = UrlEncode(item.Value(value).ToString());
                    result[item.Key.Name] = itemValue;
                }
                return result;
            }

            private static PropertyGetter CreatePropertyGetter(PropertyInfo property)
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
        }
    }
}
