using Domain.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Core.Entities.OrderAggrigate
{
    public class OrderItem:Base.BaseEntity
    {
        private string _productName;
        private decimal _unitPrice;
        private decimal _discount;
        private int _units;
        private bool _breakable;
        private decimal _profitAmount;

        public int ProductId { get; private set; }

        protected OrderItem() { }
        public OrderItem(int productId,string productName,decimal unitPrice,decimal discount,bool breakable = false,int units=1,decimal profitAmount=0)
        {
            if(units<0)
            {
                throw new OrderException("تعداد قلم سفارش نادرست است");
            }
            if ((unitPrice * units) < discount)
            {
                throw new OrderException("قیمت کالای انتخابی کمتر از تخفیف است");
            }
            _productName = productName;
            _unitPrice = unitPrice;
            _discount = discount;
            _profitAmount = profitAmount;
            _units = units;
            _breakable = breakable;

        }

        public bool GetProductType()
        {
            return _breakable;
        }

        public decimal GetCurrentDiscount()
        {
            return _discount;
        }

        public int GetUnits()
        {
            return _units;
        }

        public decimal GetUnitPrice()
        {
            return _unitPrice + _profitAmount;
        }

        public string GetOrderItemProductName() => _productName;

        public void SetNewDiscount(decimal discount)
        {
            if (discount < 0)
            {
                throw new OrderException("Discount is not valid");
            }

            _discount = discount;
        }

        public void AddUnits(int units)
        {
            if (units < 0)
            {
                throw new OrderException("Invalid units");
            }

            _units += units;
        }
    }
}
