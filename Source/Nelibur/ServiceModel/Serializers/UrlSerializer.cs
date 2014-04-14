using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Web;
using Nelibur.Core;
using Nelibur.Core.Reflection;
using Nelibur.ServiceModel.Contracts;

namespace Nelibur.ServiceModel.Serializers
{
    internal sealed class UrlSerializer
    {
        private static readonly Dictionary<Type, ObjectCreator> _objectCreators = new Dictionary<Type, ObjectCreator>();
        private static readonly Dictionary<Type, QueryStringCreator> _queryStringCreators = new Dictionary<Type, QueryStringCreator>();

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
            QueryStringCreator queryStringCreator;
            if (!_queryStringCreators.TryGetValue(typeof(T), out queryStringCreator))
            {
                queryStringCreator = new QueryStringCreator(typeof(T));
                _queryStringCreators[typeof(T)] = queryStringCreator;
            }
            NameValueCollection collection = queryStringCreator.GetNameValueCollection(value);
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
                    string value = UrlDecode(collection[key]);
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
                    string itemValue = UrlEncode(item.Value(value).ToString());
                    result[item.Key.Name] = itemValue;
                }
                return result;
            }
        }
    }
}
