using System;

namespace SpecFlowTests.Samples.JsonService
{
    public sealed class OrderJson
    {
        public int Id { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
