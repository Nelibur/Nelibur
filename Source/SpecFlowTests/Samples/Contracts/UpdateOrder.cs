using System.Runtime.Serialization;

namespace SpecFlowTests.Samples.Contracts
{
    [DataContract]
    public sealed class UpdateOrder
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int Quantity { get; set; }
    }
}
