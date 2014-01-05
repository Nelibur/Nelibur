using System;
using System.ServiceModel.Channels;

namespace Nelibur.ServiceModel.Services.Headers
{
    internal sealed class RestContentTypeHeader : RestHttpRequestHeader
    {
        private const string NameValue = "nelibur-content-type";

        public RestContentTypeHeader(Type contentType)
        {
            Value = contentType.Name;
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
    }
}
