using Domain.Core.Contracts;
using Domain.Core.Events;
using Domain.Core.Exceptions;
using Domain.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Core.Entities.OrderAggrigate
{
    public class Order : Base.BaseEntity, IAggregateRoot
    {
        public  DateTime OrderDate { get; private set; }
        public string Description { get; private set; }

        public Address Address { get;private set; }
        public PostMethod PostMethod { get; private set; }
        public PackagingMethod PackagingMethod { get; private set; }

        private readonly List<OrderItem> _orderItems;
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

        public int? GetById => _buyerId;
        private int? _buyerId;

        protected Order()
        {
            _orderItems = new List<OrderItem>();
        }

        public Order(string userId, string userName,Address address,PostMethod postMethod,PackagingMethod packagingMethod, int? buyerId = null) : this()
        {
            _buyerId = buyerId;
            Address = address;
            PackagingMethod = packagingMethod;
            PostMethod = postMethod;
            OrderDate = DateTime.UtcNow;
            AddOrderStartedDomainEvent(userId, userName);

        }

        public void AddOrderItem(int productId, string productName, decimal unitPrice, decimal discount, int units = 1,decimal profitAmount=0)
        {
            var existingOrderForProduct = _orderItems.Where(o => o.ProductId == productId)
                .SingleOrDefault();

            if (existingOrderForProduct != null)
            {
                if(profitAmount != 0)
                {
                    existingOrderForProduct.AddProfit(profitAmount);
                }

                if (discount > existingOrderForProduct.GetCurrentDiscount())
                {
                    existingOrderForProduct.SetNewDiscount(discount);
                }

                existingOrderForProduct.AddUnits(units);
            }
            else
            {
               
                var orderItem = new OrderItem(productId, productName, unitPrice, discount, units);
                _orderItems.Add(orderItem);
            }
        }

        public void SetBuyerId(int id)
        {
            _buyerId = id;
        }

        private void AddOrderStartedDomainEvent(string userId, string userName)
        {
            var orderStartedDomainEvent = new OrderStartedDomainEvent(this, userId, userName);

            this.AddDomainEvent(orderStartedDomainEvent);
        }
        public void SetShippedStatus()
        {
            Description = "The order was shipped.";
            AddDomainEvent(new OrderShippedDomainEvent(this));
        }
        public decimal GetTotal()
        {
            return _orderItems.Sum(o => o.GetUnits() * o.GetUnitPrice()); 
        }

    }
}
