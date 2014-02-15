using System;
using System.Runtime.Serialization;

namespace SpecFlowTests.Samples.JsonService
{
    [DataContract]
    public sealed class CreateOrderJson
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public Guid ProductId { get; set; }

        [DataMember]
        public int Quantity { get; set; }
    }
}
