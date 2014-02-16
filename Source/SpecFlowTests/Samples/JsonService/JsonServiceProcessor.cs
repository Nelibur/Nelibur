using System.Collections.Generic;
using Nelibur.ServiceModel.Services.Operations;

namespace SpecFlowTests.Samples.JsonService
{
    public sealed class JsonServiceProcessor : IPost<CreateOrderJson>, IPostWithResponse<CreateOrderJson>
    {
        private static readonly List<OrderJson> _repository = new List<OrderJson>();

        public void Post(CreateOrderJson request)
        {
            OrderJson order = CreateOrder(request);
            _repository.Add(order);
        }

        public object PostWithResponse(CreateOrderJson request)
        {
            OrderJson order = CreateOrder(request);
            _repository.Add(order);
            return true;
        }

        private static OrderJson CreateOrder(CreateOrderJson request)
        {
            return new OrderJson
                {
                    Id = request.Id,
                    Quantity = request.Quantity,
                    ProductId = request.ProductId
                };
        }
    }
}
