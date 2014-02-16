using System.Runtime.Serialization;

namespace SpecFlowTests.Samples.JsonService
{
    [DataContract]
    public sealed class GetOrderJsonById
    {
        [DataMember]
        public int Id { get; set; }
    }
}
