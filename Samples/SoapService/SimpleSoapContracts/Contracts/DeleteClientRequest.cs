﻿using System;
using System.Runtime.Serialization;

namespace SimpleSoapContracts.Contracts
{
    [DataContract]
    public sealed class DeleteClientRequest
    {
        [DataMember]
        public Guid Id { get; set; }

        public override string ToString()
        {
            return string.Format("Id: {0}", Id);
        }
    }
}
