using System.Collections.Generic;
using Nelibur.ServiceModel.Services.Operations;

namespace SpecFlowTests.Samples.JsonService
{
    public sealed class JsonServiceProcessor : IPost<CreateOrderJson>
    {
        private static readonly List<OrderJson> _repository = new List<OrderJson>();

        public void Post(CreateOrderJson request)
        {
            var order = new OrderJson
                {
                    Id = request.Id,
                    Quantity = request.Quantity,
                    ProductId = request.ProductId
                };
            _repository.Add(order);
        }
    }
}
