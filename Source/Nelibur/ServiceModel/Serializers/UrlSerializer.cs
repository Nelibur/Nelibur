using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using Nelibur.Core;
using Nelibur.Core.DataStructures;
using Nelibur.Core.Reflection;
using Nelibur.ServiceModel.Contracts;

namespace Nelibur.ServiceModel.Serializers
{
    internal sealed class UrlSerializer
    {
        private static readonly SafeDictionary<Type, ObjectCreator> _objectCreators = new SafeDictionary<Type, ObjectCreator>();
        private static readonly SafeDictionary<Type, QueryStringCreator> _queryStringCreators = new SafeDictionary<Type, QueryStringCreator>();

        private UrlSerializer(NameValueCollection value)
        {
            QueryParams = value;
        }

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
            QueryStringCreator queryStringCreator = _queryStringCreators.GetOrAdd(typeof(T), x => new QueryStringCreator(typeof(T)));
            NameValueCollection collection = queryStringCreator.GetNameValueCollection(value);
            return new UrlSerializer(collection);
        }

        public object GetRequestValue(Type targetType)
        {
            ObjectCreator objectCreator = _objectCreators.GetOrAdd(targetType, x => new ObjectCreator(targetType));
            return objectCreator.Create(QueryParams);
        }

        public string GetTypeValue()
        {
            const string Key = RestServiceMetadata.ParamName.Type;
            return UrlEncoder.Decode(QueryParams[Key]);
        }

        private static NameValueCollection CreateQueryParams(Type value)
        {
            return new NameValueCollection
                   {
                       { RestServiceMetadata.ParamName.Type, UrlEncoder.Encode(value.Name) },
                   };
        }

        private sealed class ObjectCreator
        {
            private readonly ObjectActivator _objectActivator;
            private readonly Dictionary<string, PropertySetter> _setters;

            public ObjectCreator(Type type)
            {
                _objectActivator = DelegateFactory.CreateCtor(type);
                _setters = type
                    .GetTypeInfo()
                    .GetProperties()
                    .ToDictionary(x => x.Name, x => DelegateFactory.CreatePropertySetter(x), StringComparer.OrdinalIgnoreCase);
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
                    string value = UrlEncoder.Decode(collection[key]);
                    _setters[key](result, value);
                }
                return result;
            }
        }

        private sealed class QueryStringCreator
        {
            private readonly Dictionary<PropertyInfo, PropertyGetter> _getters;
            private readonly NameValueCollection _typeInfo;

            public QueryStringCreator(Type value)
            {
                _typeInfo = CreateQueryParams(value);
                _getters = value
                    .GetTypeInfo()
                    .GetProperties()
                    .ToDictionary(x => x, x => DelegateFactory.CreatePropertyGetter(x));
            }

            public NameValueCollection GetNameValueCollection(object value)
            {
                var result = new NameValueCollection(_typeInfo);
                foreach (KeyValuePair<PropertyInfo, PropertyGetter> item in _getters)
                {
                    string itemValue = UrlEncoder.Encode(item.Value(value).ToString());
                    result[item.Key.Name] = itemValue;
                }
                return result;
            }
        }
    }
}
