using System;
using System.Collections.Specialized;
using Nelibur.ServiceModel.Contracts;
using Nelibur.Sword;
using Nelibur.Sword.DataStructures;

namespace Nelibur.ServiceModel.Serializers
{
    internal static class UrlSerializer
    {
        private static readonly SafeDictionary<Type, ObjectCreator> _objectCreators = new SafeDictionary<Type, ObjectCreator>();
        private static readonly SafeDictionary<Type, QueryStringCreator> _queryStringCreators = new SafeDictionary<Type, QueryStringCreator>();

        public static IUrlSerializer FromQueryParams(NameValueCollection value)
        {
            if (value == null)
            {
                throw Error.ArgumentNull("value");
            }
            return new UrlSerializerInternal(value);
        }

        public static IUrlSerializer FromType(Type value)
        {
            if (value == null)
            {
                throw Error.ArgumentNull("value");
            }
            NameValueCollection collection = CreateQueryParams(value);
            return new UrlSerializerInternal(collection);
        }

        public static IUrlSerializer FromValue(object value)
        {
            if (value == null)
            {
                throw Error.ArgumentNull("value");
            }
            QueryStringCreator queryStringCreator = GetQueryStringCreator(value.GetType());
            NameValueCollection collection = queryStringCreator.Create(value);
            return new UrlSerializerInternal(collection);
        }

        private static NameValueCollection CreateQueryParams(Type value)
        {
            return new NameValueCollection
            {
                { RestServiceMetadata.ParamName.Type, UrlEncoder.Encode(value.Name) },
            };
        }

        private static ObjectCreator GetObjectCreator(Type value)
        {
            return _objectCreators.GetOrAdd(value, x => new ObjectCreator(value));
        }

        private static QueryStringCreator GetQueryStringCreator(Type value)
        {
            return _queryStringCreators.GetOrAdd(value, x => new QueryStringCreator(value));
        }

        private sealed class UrlSerializerInternal : IUrlSerializer
        {
            public UrlSerializerInternal(NameValueCollection value)
            {
                QueryParams = value;
            }

            public NameValueCollection QueryParams { get; private set; }

            public object GetRequestValue(Type targetType)
            {
                ObjectCreator objectCreator = GetObjectCreator(targetType);
                return objectCreator.Create(QueryParams);
            }

            public string GetTypeValue()
            {
                const string Key = RestServiceMetadata.ParamName.Type;
                return UrlEncoder.Decode(QueryParams[Key]);
            }
        }
    }
}
