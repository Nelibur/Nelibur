using System;
using System.ServiceModel.Channels;
using System.Xml;

using Nelibur.ServiceModel.Services.Operations;

namespace Nelibur.ServiceModel.Services.Headers
{
    internal sealed class SoapOperationTypeHeader : MessageHeader
    {
        private const string NameValue = "nelibur-operation-type";
        private const string NamespaceValue = "http://nelibur/" + NameValue;
        private readonly string _actionType;

        private SoapOperationTypeHeader(string action)
        {
            _actionType = action;
        }

        public static SoapOperationTypeHeader Delete
        {
            get { return new SoapOperationTypeHeader(OperationType.Delete); }
        }

        public static SoapOperationTypeHeader Get
        {
            get { return new SoapOperationTypeHeader(OperationType.Get); }
        }

        public static SoapOperationTypeHeader Post
        {
            get { return new SoapOperationTypeHeader(OperationType.Post); }
        }

        public static SoapOperationTypeHeader Put
        {
            get { return new SoapOperationTypeHeader(OperationType.Put); }
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
            writer.WriteString(_actionType);
        }
    }
}
