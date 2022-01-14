using Domain.Core.Contracts;
using Domain.Core.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplication.Services.DomainEventHandler
{
    public class UpdateOrderWhenBuyerVerifiedDomainEventHandler
        : INotificationHandler<BuyerVerifiedDomainEvent>
    {

        private readonly IOrderRepository _orderRepository;

        public UpdateOrderWhenBuyerVerifiedDomainEventHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        }
        public async Task Handle(BuyerVerifiedDomainEvent buyerVerifiedDomainEvent, CancellationToken cancellationToken)
        {
            var orderToUpdate = await _orderRepository.GetAsync(buyerVerifiedDomainEvent.OrderId);
            orderToUpdate.SetBuyerId(buyerVerifiedDomainEvent.Buyer.Id);
        }
    }
}
