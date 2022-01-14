using Domain.Core.Contracts;
using Domain.Core.Entities.OrderAggrigate;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.EF.Repositories
{
    public class OrderRepository: IOrderRepository
    {
        private readonly EFContext _context;

        public IUnitOfWork UnitOfWork
        {
            get
            {
                return _context;
            }
        }

        public OrderRepository(EFContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Order Add(Order order)
        {
            return _context.Orders.Add(order).Entity;

        }

        public async Task<Order> GetAsync(int orderId)
        {
            var order = await _context
                                .Orders
                                .Include(x => x.Address)
                                .FirstOrDefaultAsync(o => o.Id == orderId);
            if (order == null)
            {
                order = _context
                            .Orders
                            .Local
                            .FirstOrDefault(o => o.Id == orderId);
            }
            if (order != null)
            {
                await _context.Entry(order)
                    .Collection(i => i.OrderItems).LoadAsync();
            }

            return order;
        }

        public void Update(Order order)
        {
            _context.Entry(order).State = EntityState.Modified;
        }

        public async Task<List<Order>> GetAllAsync()
        {
            var order = await _context
                              .Orders
                              .Include(x => x.Address)
                              .Include(i => i.OrderItems).ToListAsync();
            return order;
        }
    }
 
}
