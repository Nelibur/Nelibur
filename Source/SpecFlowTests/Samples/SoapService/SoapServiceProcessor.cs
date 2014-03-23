using System.Collections.Generic;
using System.Linq;
using Nelibur.ServiceModel.Services.Operations;
using SpecFlowTests.Samples.Contracts;

namespace SpecFlowTests.Samples.SoapService
{
    public sealed class SoapServiceProcessor : IPost<Order>,
        IPostWithResponse<Order>,
        IGetWithResponse<GetOrderById>,
        IDelete<DeleteOrderById>,
        IDeleteWithResponse<DeleteOrderById>,
        IPut<UpdateOrder>,
        IPutWithResponse<UpdateOrder>
    {
        private static List<Order> _repository = new List<Order>();

        public void Delete(DeleteOrderById request)
        {
            _repository = _repository.Where(x => x.Id != request.Id).ToList();
        }

        public object DeleteWithResponse(DeleteOrderById request)
        {
            Delete(request);
            return true;
        }

        public object GetWithResponse(GetOrderById request)
        {
            return _repository.Where(x => x.Id == request.Id).ToList();
        }

        public void Post(Order request)
        {
            _repository.Add(request);
        }

        public object PostWithResponse(Order request)
        {
            Post(request);
            return true;
        }

        public void Put(UpdateOrder request)
        {
            Order order = _repository.Single(x => x.Id == request.Id);
            order.Quantity = request.Quantity;
        }

        public object PutWithResponse(UpdateOrder request)
        {
            Put(request);
            return true;
        }
    }
}
