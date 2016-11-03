using System;
using System.Runtime.Serialization;

namespace SimpleRestContracts.Contracts
{
    [DataContract]
    public sealed class UploadRequest
    {
        [DataMember]
        public string UploaderId { get; set; }

        [DataMember]
        public string OriginalFileName { get; set; }

        [DataMember]
        public byte[] FileContents { get; set; }
    }
}
