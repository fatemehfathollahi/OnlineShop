using Domain.Core.Entities.BuyerAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Core.Events
{
    public class BuyerVerifiedDomainEvent : INotification
    {
        public Buyer Buyer { get; private set; }
        public int OrderId { get; private set; }

        public BuyerVerifiedDomainEvent(Buyer buyer, int orderId)
        {
            Buyer = buyer;
            OrderId = orderId;
        }
    }
}
