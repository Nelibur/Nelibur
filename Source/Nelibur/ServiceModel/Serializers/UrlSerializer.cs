using System;
using System.Collections.Specialized;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;

using Nelibur.ServiceModel.Contracts;

namespace Nelibur.ServiceModel.Serializers
{
    internal sealed class UrlSerializer
    {
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

        public object GetRequestValue(Type targetType)
        {
            const string Key = RestServiceMetadata.ParamName.Data;
            var serializer = new DataContractJsonSerializer(targetType);
            byte[] rawObj = Encoding.UTF8.GetBytes(UrlDecode(QueryParams[Key]));
            return serializer.ReadObject(new MemoryStream(rawObj));
        }

        public string GetTypeValue()
        {
            const string Key = RestServiceMetadata.ParamName.Type;
            return UrlDecode(QueryParams[Key]);
        }

        private static NameValueCollection CreateQueryParams<T>(T value)
        {
            string data = JsonDataSerializer.ToString(value);
            var result = new NameValueCollection
                         {
                             CreateQueryParams(typeof(T)),
                             { RestServiceMetadata.ParamName.Data, UrlEncode(data) }
                         };
            return result;
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
    }
}
