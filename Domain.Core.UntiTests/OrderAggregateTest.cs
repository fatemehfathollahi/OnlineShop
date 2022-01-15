using Domain.Core.Entities.OrderAggrigate;
using Domain.Core.Exceptions;
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

            //Act 
            var fakeOrderItem = new OrderItem(productId, productName, unitPrice, discount, units);

            //Assert
            Assert.NotNull(fakeOrderItem);
        }

        [Fact]
        public void Invalid_number_of_units()
        {
            //Arrange    
            var productId = 1;
            var productName = "FakeProductName";
            var unitPrice = 12;
            var discount = 15;
            var units = -1;

            //Act - Assert
            Assert.Throws<OrderException>(() => new OrderItem(productId, productName, unitPrice, discount, units));
        }

        [Fact]
        public void Invalid_units_setting()
        {
            //Arrange    
            var productId = 1;
            var productName = "FakeProductName";
            var unitPrice = 12;
            var discount = 15;
            var units = 5;

            //Act 
            var fakeOrderItem = new OrderItem(productId, productName, unitPrice, discount, units);

            //Assert
            Assert.Throws<OrderException>(() => fakeOrderItem.AddUnits(-1));
        }

        [Fact]
        public void Invalid_total_of_order_item_more_than_fiftythousand()
        {
            //Arrange    
            var productId = 1;
            var productName = "FakeProductName";
            var unitPrice = 12;
            var discount = 15;
            var units = 1;

            //Act - Assert
            Assert.Throws<OrderException>(() => new OrderItem(productId, productName, unitPrice, discount, units));
        }

        [Fact]
        public void when_add_two_times_on_the_same_item_then_the_total_of_order_should_be_the_sum_of_the_two_items()
        {
            var address = new AddressBuilder().Build();
            var postMethod = new PostMethodBuilder().Build();
            var packageMethod = new PackagingBuilder().Build();
            var order = new OrderBuilder(address, packageMethod, postMethod)
                .AddOne(1, "cup", 10.0m, 0, string.Empty)
                .AddOne(1, "cup", 10.0m, 0, string.Empty)
                .Build();

            Assert.Equal(20.0m, order.GetTotal());
        }

        [Fact]
        public void Invalid_ordering_time()
        {
            var address = new AddressBuilder().Build();
            var postMethod = new PostMethodBuilder().Build();
            var packageMethod = new PackagingBuilder().Build();
            var order = new OrderBuilder(address, packageMethod, postMethod)
                .AddOne(1, "cup", 10.0m, 0, string.Empty)
                .Build(); Assert.Equal(20.0m, order.GetTotal());

        }

        
    }
}
