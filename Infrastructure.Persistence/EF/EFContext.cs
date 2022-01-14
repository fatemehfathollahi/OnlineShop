using Aplication.Services;
using Domain.Core.Contracts;
using Domain.Core.Entities.BuyerAggregate;
using Domain.Core.Entities.OrderAggrigate;
using Infrastructure.Persistence.EF.EnitiesConfig;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.EF
{
    public class EFContext : DbContext, IApplicationDbContext
    {
        public const string DEFAULT_SCHEMA = "ordering";
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Buyer> Buyers { get; set; }


        
        private IDbContextTransaction _currentTransaction;

        public EFContext(DbContextOptions<EFContext> options) : base(options) { }

        public IDbContextTransaction dbContextTransaction => _currentTransaction = null;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OrderConfig());
            modelBuilder.ApplyConfiguration(new OrderItemConfig());
        }
        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            var result = await base.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (_currentTransaction != null) return null;

            _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

            return _currentTransaction;
        }

        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

            try
            {
                await SaveChangesAsync();
                transaction.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

    }

    public class EFContextDesignFactory : IDesignTimeDbContextFactory<EFContext>
    {
        public EFContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EFContext>()
                .UseSqlServer("Server=.;Initial Catalog=OnlineShop;Integrated Security=true");

            return new EFContext(optionsBuilder.Options);
        }

        //class NoMediator : IMediator
        //{
        //    public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default(CancellationToken)) where TNotification : INotification
        //    {
        //        return Task.CompletedTask;
        //    }

        //    public Task Publish(object notification, CancellationToken cancellationToken = default)
        //    {
        //        return Task.CompletedTask;
        //    }

        //    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default(CancellationToken))
        //    {
        //        return Task.FromResult<TResponse>(default(TResponse));
        //    }

        //    public Task<object> Send(object request, CancellationToken cancellationToken = default)
        //    {
        //        return Task.FromResult(default(object));
        //    }
        //}
    }
}
