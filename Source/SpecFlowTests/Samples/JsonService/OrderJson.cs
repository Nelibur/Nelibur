using System;
using System.Runtime.Serialization;

namespace SpecFlowTests.Samples.JsonService
{
    [DataContract]
    public sealed class OrderJson
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public Guid ProductId { get; set; }

        [DataMember]
        public int Quantity { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            return obj is OrderJson && Equals((OrderJson)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Id;
                hashCode = (hashCode*397) ^ Quantity;
                hashCode = (hashCode*397) ^ ProductId.GetHashCode();
                return hashCode;
            }
        }

        private bool Equals(OrderJson other)
        {
            return Id == other.Id && Quantity == other.Quantity && ProductId.Equals(other.ProductId);
        }
    }
}
