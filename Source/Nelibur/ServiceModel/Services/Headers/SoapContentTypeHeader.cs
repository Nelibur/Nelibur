using System;
using System.ServiceModel.Channels;
using System.Xml;

namespace Nelibur.ServiceModel.Services.Headers
{
    internal sealed class SoapContentTypeHeader : MessageHeader
    {
        private const string NameValue = "nelibur-content-type";
        private const string NamespaceValue = "http://nelibur.org/" + NameValue;
        private readonly string _contentType;

        public SoapContentTypeHeader(Type contentType)
        {
            _contentType = contentType.Name;
        }

        public override string Name
        {
            get { return NameValue; }
        }

        public override string Namespace
        {
            get { return NamespaceValue; }
        }

        public static string ReadHeader(Message request)
        {
            int headerPosition = request.Headers.FindHeader(NameValue, NamespaceValue);
            if (headerPosition == -1)
            {
                return null;
            }
            var content = request.Headers.GetHeader<string>(headerPosition);
            return content;
        }

        protected override void OnWriteHeaderContents(XmlDictionaryWriter writer, MessageVersion messageVersion)
        {
            writer.WriteString(_contentType);
        }
    }
}
