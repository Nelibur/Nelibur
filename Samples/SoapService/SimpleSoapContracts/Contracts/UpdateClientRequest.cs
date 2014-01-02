using System;
using System.Runtime.Serialization;

namespace SimpleSoapContracts.Contracts
{
    [DataContract]
    public sealed class UpdateClientRequest
    {
        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public Guid Id { get; set; }

        public override string ToString()
        {
            return string.Format("Id: {0}, Email: {1}", Id, Email);
        }
    }
}
