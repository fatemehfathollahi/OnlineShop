using Domain.Core.Entities.OrderAggrigate;
using Domain.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Core.UntiTests
{
    public class AddressBuilder
    {
        public Address Build()
        {
            return new Address("street", "city", "state", "country", "zipcode");
        }
    }

    public class PostMethodBuilder
    {
        public PostMethod Build()
        {
            return new PostMethod(PostMethod.PostType.Normal);
        }
    }

    public class PackagingBuilder
    {
        public PackagingMethod Build()
        {
            return new PackagingMethod(PackagingMethod.PackagingType.Normal);
        }
    }

    public class OrderBuilder
    {
        private readonly Order order;

        public OrderBuilder(Address address,PackagingMethod packagingMethod,PostMethod postMethod)
        {
            order = new Order(
                "userId",
                "fakeName",
                address,
                postMethod,
                packagingMethod
                );
        }

        public OrderBuilder AddOne(
            int productId,
            string productName,
            decimal unitPrice,
            decimal discount,
            string pictureUrl,
            int units = 1)
        {
            order.AddOrderItem(productId, productName, unitPrice, discount, units);
            return this;
        }

        public Order Build()
        {
            return order;
        }
    }
}
