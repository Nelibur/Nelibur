using System.Runtime.Serialization;

namespace SpecFlowTests.Samples.Contracts
{
    [DataContract]
    public sealed class DeleteOrderById
    {
        [DataMember]
        public int Id { get; set; }
    }
}