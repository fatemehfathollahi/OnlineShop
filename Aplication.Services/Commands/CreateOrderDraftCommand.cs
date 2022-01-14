using Aplication.Services.Extensions;
using Aplication.Services.Models;
using Domain.Core.Contracts;
using Domain.Core.Entities.OrderAggrigate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Aplication.Services.Commands.CreateOrderCommand;
using static Aplication.Services.Commands.CreateOrderDraftCommand;

namespace Aplication.Services.Commands
{
    public class CreateOrderDraftCommand : IRequest<OrderDraftDTO>
    {

        public string BuyerId { get; private set; }

        public IEnumerable<BasketItem> Items { get; private set; }

        public CreateOrderDraftCommand(string buyerId, IEnumerable<BasketItem> items)
        {
            BuyerId = buyerId;
            Items = items;
        }



        public record OrderDraftDTO
        {
            public IEnumerable<OrderItemDTO> OrderItems { get; init; }
            public decimal Total { get; init; }

            public static OrderDraftDTO FromOrder(Order order)
            {
                return new OrderDraftDTO()
                {
                    OrderItems = order.OrderItems.Select(oi => new OrderItemDTO
                    {
                        Discount = oi.GetCurrentDiscount(),
                        ProductId = oi.ProductId,
                        UnitPrice = oi.GetUnitPrice(),
                        Units = oi.GetUnits(),
                        ProductName = oi.GetOrderItemProductName()
                    }),
                    Total = order.GetTotal()
                };
            }

        }


        public class CreateOrderDraftCommandHandler
      : IRequestHandler<CreateOrderDraftCommand, OrderDraftDTO>
        {
            private readonly IOrderRepository _orderRepository;
            private readonly IMediator _mediator;

            // Using DI to inject infrastructure persistence Repositories
            public CreateOrderDraftCommandHandler(IMediator mediator)
            {
                _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            }

            public Task<OrderDraftDTO> Handle(CreateOrderDraftCommand message, CancellationToken cancellationToken)
            {

                var order = Order.NewDraft();
                var orderItems = message.Items.Select(i => i.ToOrderItemDTO());
                foreach (var item in orderItems)
                {
                    order.AddOrderItem(item.ProductId, item.ProductName, item.UnitPrice, item.Discount,item.Breakable, item.Units);
                }

                return Task.FromResult(OrderDraftDTO.FromOrder(order));
            }
        }

    }


}
