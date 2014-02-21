using System.Collections.Generic;
using System.Linq;
using Nelibur.ServiceModel.Services.Operations;

namespace SpecFlowTests.Samples.JsonService
{
    public sealed class JsonServiceProcessor : IPost<OrderJson>,
        IPostWithResponse<OrderJson>,
        IGetWithResponse<GetOrderJsonById>,
        IDelete<DeleteOrderJsonById>,
        IDeleteWithResponse<DeleteOrderJsonById>,
        IPut<UpdateOrderJson>,
        IPutWithResponse<UpdateOrderJson>
    {
        private static List<OrderJson> _repository = new List<OrderJson>();

        public void Delete(DeleteOrderJsonById request)
        {
            _repository = _repository.Where(x => x.Id != request.Id).ToList();
        }

        public object DeleteWithResponse(DeleteOrderJsonById request)
        {
            Delete(request);
            return true;
        }

        public object GetWithResponse(GetOrderJsonById request)
        {
            return _repository.Where(x => x.Id == request.Id).ToList();
        }

        public void Post(OrderJson request)
        {
            _repository.Add(request);
        }

        public object PostWithResponse(OrderJson request)
        {
            Post(request);
            return true;
        }

        public void Put(UpdateOrderJson request)
        {
            OrderJson order = _repository.Single(x => x.Id == request.Id);
            order.Quantity = request.Quantity;
        }

        public object PutWithResponse(UpdateOrderJson request)
        {
            Put(request);
            return true;
        }
    }
}
