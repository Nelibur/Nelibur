using System.Runtime.Serialization;

namespace SpecFlowTests.Samples.Contracts
{
    [DataContract]
    public sealed class GetOrderById
    {
        [DataMember]
        public int Id { get; set; }
    }
}
