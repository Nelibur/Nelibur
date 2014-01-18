using System;
using System.Collections.Specialized;
using System.IO;
using System.Runtime.Serialization.Json;
using System.ServiceModel.Dispatcher;
using Nelibur.ServiceModel.Contracts;

namespace Nelibur.ServiceModel.Serializers
{
    public sealed class UrlSerializer
    {
        private static readonly QueryStringConverter _converter = new QueryStringConverter();

        private UrlSerializer(NameValueCollection value)
        {
            QueryParams = value;
        }

        public NameValueCollection QueryParams { get; private set; }

        public static UrlSerializer FromQueryParams(NameValueCollection value)
        {
            if (value == null)
            {
                throw new ArgumentNullException();
            }
            return new UrlSerializer(value);
        }

        public static UrlSerializer FromType(Type value)
        {
            if (value == null)
            {
                throw new ArgumentNullException();
            }
            NameValueCollection collection = CreateQueryParams(value);
            return new UrlSerializer(collection);
        }

        public static UrlSerializer FromValue<T>(T value)
            where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException();
            }
            NameValueCollection collection = CreateQueryParams(value);
            return new UrlSerializer(collection);
        }

        public object GetRequest(Type targetType)
        {
            const string Key = RestServiceMetadata.ParamNames.Request;
            var serializer = new DataContractJsonSerializer(targetType);
            object rawObj = _converter.ConvertStringToValue(QueryParams[Key], typeof(byte[]));
            return serializer.ReadObject(new MemoryStream((byte[])rawObj));
        }

        public string GetTypeValue()
        {
            const string Key = RestServiceMetadata.ParamNames.Type;
            return (string)_converter.ConvertStringToValue(QueryParams[Key], typeof(string));
        }

        private static NameValueCollection CreateQueryParams<T>(T value)
        {
            byte[] data = JsonDataSerializer.ToByte(value);
            string requestData = _converter.ConvertValueToString(data, typeof(byte[]));
            var result = new NameValueCollection
                {
                    CreateQueryParams(typeof(T)),
                    { RestServiceMetadata.ParamNames.Request, requestData }
                };
            return result;
        }

        private static NameValueCollection CreateQueryParams(Type value)
        {
            string requestType = value.Name;
            return new NameValueCollection
                {
                    { RestServiceMetadata.ParamNames.Type, requestType },
                };
        }
    }
}
