using System.ServiceModel.Channels;
using System.Xml;
using Nelibur.ServiceModel.Services.Operations;

namespace Nelibur.ServiceModel.Services.Headers
{
    internal sealed class OperationTypeHeader : MessageHeader
    {
        private const string NameValue = "nelibur-operation-type";
        private const string NamespaceValue = "http://nelibur/" + NameValue;
        private readonly string _actionType;

        private OperationTypeHeader(string action)
        {
            _actionType = action;
        }

        public static OperationTypeHeader Delete
        {
            get { return new OperationTypeHeader(OperationType.Delete); }
        }

        public static OperationTypeHeader Get
        {
            get { return new OperationTypeHeader(OperationType.Get); }
        }

        public static OperationTypeHeader Post
        {
            get { return new OperationTypeHeader(OperationType.Post); }
        }

        public static OperationTypeHeader Put
        {
            get { return new OperationTypeHeader(OperationType.Put); }
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
