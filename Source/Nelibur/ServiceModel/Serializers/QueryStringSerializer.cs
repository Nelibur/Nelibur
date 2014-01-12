using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.ServiceModel.Dispatcher;

namespace Nelibur.ServiceModel.Serializers
{
    public sealed class QueryStringSerializer
    {
        private static readonly JsonDataSerializer _jsondataSerializer = new JsonDataSerializer();

        public object ToObject(Type targetType, string url)
        {
            var serializer = new DataContractJsonSerializer(targetType);
            var converter = new QueryStringConverter();
            object rawObj = converter.ConvertStringToValue(url, typeof(byte[]));
            return serializer.ReadObject(new MemoryStream((byte[])rawObj));
        }

        public string ToUrl<T>(T value)
        {
            byte[] data = _jsondataSerializer.ToByte(value);
            var converter = new QueryStringConverter();
            return converter.ConvertValueToString(data, typeof(byte[]));
        }
    }
}
