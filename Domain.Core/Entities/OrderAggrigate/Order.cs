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
        private DateTime _orderDate;

        public Address Address { get;private set; }

        public int? GetById => _buyerId;
        private int? _buyerId;

        private PostMethod _postMethod;
        private PackagingMethod _packagingMethod;

        // public OrderStatus OrderStatus { get; private set; }
        //private int _orderStatusId;

        private string _description;

        private bool _isDraft;

        private readonly List<OrderItem> _orderItems;
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

        public static Order NewDraft()
        {
            var order = new Order();
            order._isDraft = true;
            return order;
        }

        protected Order()
        {
            _orderItems = new List<OrderItem>();
            _isDraft = false;
        }

        public Order(string userId, string userName,Address address, int? buyerId = null) : this()
        {
           
            _orderDate = DateTime.UtcNow;
            //if (GetTotal() < 50000)
            //{
            //    throw new OrderException("سفارش کمتر از مبلغ 50000 امکان پذیر نمی باشد");
            //}
            //TimeSpan time = _orderDate.TimeOfDay;
            //if (time < new TimeSpan(08, 00, 00) || time > new TimeSpan(19, 00, 00))
            //{
            //    throw new OrderException("بازه ثبت سفارش از 8 صبح تا 7 بعد از ظهر امکان پذیر میباشد");
            //}

            _buyerId = buyerId;
            Address = address;
            AddOrderStartedDomainEvent(userId, userName);

        }

        public void AddOrderItem(int productId, string productName, decimal unitPrice, decimal discount,bool breakable=false, int units = 1,decimal profitAmount=0)
        {
            var existingOrderForProduct = _orderItems.Where(o => o.ProductId == productId)
                .SingleOrDefault();

            if (existingOrderForProduct != null)
            {
                //if previous line exist modify it with higher discount  and units..

                if (discount > existingOrderForProduct.GetCurrentDiscount())
                {
                    existingOrderForProduct.SetNewDiscount(discount);
                }

                existingOrderForProduct.AddUnits(units);
            }
            else
            {
                //add validated new order item

                var orderItem = new OrderItem(productId, productName, unitPrice, discount, breakable, units, profitAmount);
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
            _description = "The order was shipped.";
        }
        public decimal GetTotal()
        {
            return _orderItems.Sum(o => o.GetUnits() * o.GetUnitPrice()); 
        }
    }
}
