using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Nelibur.ServiceModel.Serializers
{
    internal static class JsonDataSerializer
    {
        public static string ToString<T>(T value)
        {
            using (var stream = new MemoryStream())
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                serializer.WriteObject(stream, value);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        public static TResponse ToValue<TResponse>(Stream value)
        {
            var serializer = new DataContractJsonSerializer(typeof(TResponse));
            return (TResponse)serializer.ReadObject(value);
        }
    }
}
