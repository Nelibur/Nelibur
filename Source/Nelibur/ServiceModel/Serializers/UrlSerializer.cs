using System;
using System.Collections.Specialized;
using Nelibur.Core;
using Nelibur.Core.DataStructures;
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
            QueryStringCreator queryStringCreator = GetQueryStringCreator(typeof(T));
            NameValueCollection collection = queryStringCreator.Create(value);
            return new UrlSerializer(collection);
        }

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

        private static NameValueCollection CreateQueryParams(Type value)
        {
            return new NameValueCollection
                   {
                       { RestServiceMetadata.ParamName.Type, UrlEncoder.Encode(value.Name) },
                   };
        }

        private static QueryStringCreator GetQueryStringCreator(Type value)
        {
            return _queryStringCreators.GetOrAdd(value, x => new QueryStringCreator(value));
        }

        private ObjectCreator GetObjectCreator(Type value)
        {
            return _objectCreators.GetOrAdd(value, x => new ObjectCreator(value));
        }
    }
}
