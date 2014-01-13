﻿using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Nelibur.ServiceModel.Serializers
{
    public static class JsonDataSerializer
    {
        public static byte[] ToByte<T>(T value)
        {
            using (var stream = new MemoryStream())
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                serializer.WriteObject(stream, value);
                return stream.ToArray();
            }
        }

        public static string ToString<T>(T value)
        {
            return Encoding.UTF8.GetString(ToByte(value));
        }
    }
}
