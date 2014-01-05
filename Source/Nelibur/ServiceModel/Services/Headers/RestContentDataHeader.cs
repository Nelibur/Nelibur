using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.ServiceModel.Channels;
using System.Text;

namespace Nelibur.ServiceModel.Services.Headers
{
    internal sealed class RestContentDataHeader : RestHttpRequestHeader
    {
        private const string NameValue = "nelibur-content-data";

        public RestContentDataHeader(object content)
        {
            if (content == null)
            {
                throw new ArgumentException("content");
            }
            Value = Serialise(content);
        }

        public override string Name
        {
            get { return NameValue; }
        }

        public override string Value { get; protected set; }

        internal static string ReadHeader(Message value)
        {
            var messageProperty = (HttpRequestMessageProperty)value.Properties[HttpRequestMessageProperty.Name];
            return messageProperty.Headers.Get(NameValue);
        }

        private static string Serialise(object value)
        {
            using (var stream = new MemoryStream())
            {
                var serializer = new DataContractJsonSerializer(value.GetType());
                serializer.WriteObject(stream, value);
                string content = Encoding.UTF8.GetString(stream.ToArray());
                return content;
            }
        }
    }
}
