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
        //[DataMember]
        //private readonly List<OrderItemDTO> _orderItems;

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

        // [DataMember]
        //  public IEnumerable<OrderItemDTO> OrderItems => _orderItems;

        [DataMember]
        public IEnumerable<OrderItemDTO> OrderItems { get; private set; }


        //public CreateOrderCommand()
        //{
        //    _orderItems = new List<OrderItemDTO>();
       // }

        public CreateOrderCommand(IEnumerable<OrderItemDTO> orderItems, string userId, string userName, string city, string street,
            string state, string country, string zipcode) //: this()
        {
            OrderItems = orderItems;
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

            public bool Breakable { get; init; }
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
                var order = new Order(command.UserId, command.UserName, address);

                foreach (var item in command.OrderItems)
                {
                    order.AddOrderItem(item.ProductId, item.ProductName, item.UnitPrice, item.Discount,item.Breakable,item.Units);
                }

                if (order.GetTotal() < 50000)
                {
                    throw new OrderException("سفارش کمتر از مبلغ 50000 امکان پذیر نمی باشد");
                }
                TimeSpan time = DateTime.Now.TimeOfDay;
                if (time < new TimeSpan(08, 00, 00) || time > new TimeSpan(19, 00, 00))
                {
                    throw new OrderException("بازه ثبت سفارش از 8 صبح تا 7 بعد از ظهر امکان پذیر میباشد");
                }

                //_logger.LogInformation("----- Creating Order - Order: {@Order}", order);

                _orderRepository.Add(order);

                return await _orderRepository.UnitOfWork
                    .SaveEntitiesAsync(cancellationToken);
            }
        }
    }
}
