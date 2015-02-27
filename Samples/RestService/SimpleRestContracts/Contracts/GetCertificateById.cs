using System;
using System.Runtime.Serialization;

namespace SimpleRestContracts.Contracts
{
    [DataContract]
    public sealed class GetCertificateById
    {
        [DataMember]
        public int Id { get; set; }
    }
}
