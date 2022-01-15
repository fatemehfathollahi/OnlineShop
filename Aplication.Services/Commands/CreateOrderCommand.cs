using Aplication.Services.Extensions;
using Aplication.Services.Models;
using Domain.Core.Contracts;
using Domain.Core.Entities.OrderAggrigate;
using Domain.Core.Exceptions;
using Domain.Core.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplication.Services.Commands
{
    public class CreateOrderCommand : IRequest<bool>
    {
        [DataMember]
        public string UserId { get; private set; }

        [DataMember]
        public string UserName { get; private set; }

        [DataMember]
        public string City { get; private set; }

        [DataMember]
        public string Street { get; private set; }

        [DataMember]
        public string State { get; private set; }

        [DataMember]
        public string Country { get; private set; }

        [DataMember]
        public string ZipCode { get; private set; }

        [DataMember]
        public DateTime OrderDate { get; private set; }

        [DataMember]
        public IEnumerable<OrderItemDTO> OrderItems { get; private set; }

        public CreateOrderCommand(IEnumerable<BasketItem> basketItems, string userId, string userName, string city, string street,
            string state, string country, string zipcode)
        {
            OrderItems = basketItems.ToOrderItemsDTO();
            UserId = userId;
            UserName = userName;
            City = city;
            Street = street;
            State = state;
            Country = country;
            ZipCode = zipcode;
            OrderDate = DateTime.UtcNow;
        }
        public record OrderItemDTO
        {
            public int ProductId { get; init; }

            public string ProductName { get; init; }

            public decimal UnitPrice { get; init; }

            public decimal Discount { get; init; }

            public int Units { get; init; }
        }

        //................................................

        public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, bool>
        {
            private readonly IOrderRepository _orderRepository;
            public CreateOrderCommandHandler(IOrderRepository orderRepository)
            {
                _orderRepository = orderRepository;
            }
            public async Task<bool> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
            {
                var address = new Address(command.Street, command.City, command.State, command.Country, command.ZipCode);
                var postType = new PostMethod(PostMethod.PostType.Normal);
                var packaging = new PackagingMethod(PackagingMethod.PackagingType.Normal);
                var order = new Order(command.UserId, command.UserName, address,postType, packaging);

                foreach (var item in command.OrderItems)
                {
                    order.AddOrderItem(item.ProductId, item.ProductName, item.UnitPrice, item.Discount,item.Units);
                }
                _orderRepository.Add(order);

                return await _orderRepository.UnitOfWork
                    .SaveEntitiesAsync(cancellationToken);
            }
        }
    }
}
