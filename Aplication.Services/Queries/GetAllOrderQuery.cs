using Domain.Core.Contracts;
using Domain.Core.Entities.OrderAggrigate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplication.Services.Queries
{
    public class GetAllOrderQuery : IRequest<IEnumerable<Order>>
    {
        public class GetAllOrderQueryHandler : IRequestHandler<GetAllOrderQuery, IEnumerable<Order>>
        {
            private readonly IOrderRepository _orderRepository;
            public GetAllOrderQueryHandler(IOrderRepository orderRepository)
            {
                _orderRepository = orderRepository;
            }
            public async Task<IEnumerable<Order>> Handle(GetAllOrderQuery query, CancellationToken cancellationToken)
            {
                var orderList = await _orderRepository.GetAllAsync();
                if (orderList == null)
                {
                    return null;
                }
                return orderList.AsReadOnly();
            }
        }
    }
}
