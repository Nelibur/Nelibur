using System;
using System.ServiceModel.Channels;

namespace Nelibur.ServiceModel.Services.Headers
{
    internal sealed class RestContentTypeHeader
    {
        private const string NameValue = "nelibur-content-type";

        public RestContentTypeHeader(Type contentType)
        {
            Value = contentType.Name;
        }

        public string Name
        {
            get { return NameValue; }
        }

        public string Value { get; private set; }

        internal static string ReadHeader(Message value)
        {
            var messageProperty = (HttpRequestMessageProperty)value.Properties[HttpRequestMessageProperty.Name];
            return messageProperty.Headers.Get(NameValue);
        }
    }
}
