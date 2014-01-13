using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.ServiceModel.Dispatcher;

namespace Nelibur.ServiceModel.Serializers
{
    public static class QueryStringSerializer
    {
        public static object ToObject(Type targetType, string url)
        {
            var serializer = new DataContractJsonSerializer(targetType);
            var converter = new QueryStringConverter();
            object rawObj = converter.ConvertStringToValue(url, typeof(byte[]));
            return serializer.ReadObject(new MemoryStream((byte[])rawObj));
        }

        public static string ToUrl<T>(T value)
        {
            byte[] data = JsonDataSerializer.ToByte(value);
            var converter = new QueryStringConverter();
            return converter.ConvertValueToString(data, typeof(byte[]));
        }
    }
}
