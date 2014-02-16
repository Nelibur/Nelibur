using System.Collections.Generic;
using System.Linq;
using Nelibur.ServiceModel.Services.Operations;

namespace SpecFlowTests.Samples.JsonService
{
    public sealed class JsonServiceProcessor : IPost<OrderJson>,
        IPostWithResponse<OrderJson>,
        IGetWithResponse<GetOrderJsonById>,
        IDelete<DeleteOrderJsonById>
    {
        private static List<OrderJson> _repository = new List<OrderJson>();

        public void Delete(DeleteOrderJsonById request)
        {
            _repository = _repository.Where(x => x.Id != request.Id).ToList();
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
            _repository.Add(request);
            return true;
        }
    }
}
