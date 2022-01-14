using Domain.Core.Contracts;
using Domain.Core.Entities.BuyerAggregate;
using Domain.Core.Entities.OrderAggrigate;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Services
{
    public interface IApplicationDbContext : IUnitOfWork
    {
        DbSet<Order> Orders { get; set; }
        DbSet<OrderItem> OrderItems { get; set; }
        DbSet<Buyer> Buyers { get; set; }
    }
}
