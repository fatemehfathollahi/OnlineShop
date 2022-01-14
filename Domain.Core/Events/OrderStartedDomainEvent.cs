using Domain.Core.Entities.OrderAggrigate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Core.Events
{
    public class OrderStartedDomainEvent: INotification
    {
        public string UserId { get; }
        public string UserName { get; }
        public Order Order { get; }

        public OrderStartedDomainEvent(Order order, string userId, string userName)
        {
            Order = order;
            UserId = userId;
            UserName = userName;
        }
    }
}
