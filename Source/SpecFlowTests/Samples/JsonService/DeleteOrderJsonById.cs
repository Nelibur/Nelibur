using System.Runtime.Serialization;

namespace SpecFlowTests.Samples.JsonService
{
    [DataContract]
    public sealed class DeleteOrderJsonById
    {
        [DataMember]
        public int Id { get; set; }
    }
}