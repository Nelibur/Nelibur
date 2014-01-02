using System;
using System.Runtime.Serialization;

namespace SimpleSoapContracts.Contracts
{
    [DataContract]
    public sealed class ClientResponse
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public string Email { get; set; }

        public override string ToString()
        {
            return string.Format("Id: {0}, Email: {1}", Id, Email);
        }
    }
}