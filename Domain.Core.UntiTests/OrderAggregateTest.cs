using Domain.Core.Entities.OrderAggrigate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Domain.Core.UntiTests
{
    public class OrderAggregateTest
    {
        public OrderAggregateTest()
        { }

        [Fact]
        public void Create_order_item_success()
        {
            //Arrange    
            var productId = 1;
            var productName = "FakeProductName";
            var unitPrice = 12;
            var discount = 15;
            var units = 50;
            var breakable = true;
            var profitAmount = 10;

            //Act 
            var fakeOrderItem = new OrderItem(productId, productName, unitPrice, discount, breakable, units,profitAmount);

            //Assert
            Assert.NotNull(fakeOrderItem);
        }
    }
}
