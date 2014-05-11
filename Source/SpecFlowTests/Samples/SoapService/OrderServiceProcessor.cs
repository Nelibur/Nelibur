using System.Collections.Generic;
using System.Linq;
using Nelibur.ServiceModel.Services.Operations;
using SpecFlowTests.Samples.Contracts;

namespace SpecFlowTests.Samples.SoapService
{
    public sealed class OrderServiceProcessor : IPostOneWay<Order>,
        IPost<Order>,
        IGet<GetOrderById>,
        IDeleteOneWay<DeleteOrderById>,
        IDelete<DeleteOrderById>,
        IPutOneWay<UpdateOrder>,
        IPut<UpdateOrder>
    {
        private static List<Order> _repository = new List<Order>();

        public void DeleteOneWay(DeleteOrderById request)
        {
            _repository = _repository.Where(x => x.Id != request.Id).ToList();
        }

        public object Delete(DeleteOrderById request)
        {
            DeleteOneWay(request);
            return true;
        }

        public object Get(GetOrderById request)
        {
            return _repository.Where(x => x.Id == request.Id).ToList();
        }

        public void PostOneWay(Order request)
        {
            _repository.Add(request);
        }

        public object Post(Order request)
        {
            PostOneWay(request);
            return true;
        }

        public void PutOneWay(UpdateOrder request)
        {
            Order order = _repository.Single(x => x.Id == request.Id);
            order.Quantity = request.Quantity;
        }

        public object Put(UpdateOrder request)
        {
            PutOneWay(request);
            return true;
        }
    }
}
