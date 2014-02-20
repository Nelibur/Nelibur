using System.Runtime.Serialization;

namespace SpecFlowTests.Samples.JsonService
{
    [DataContract]
    public sealed class UpdateOrderJson
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int Quantity { get; set; }
    }
}
