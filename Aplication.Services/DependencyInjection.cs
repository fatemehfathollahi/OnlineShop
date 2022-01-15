using Aplication.Services.Commands;
using Aplication.Services.DomainEventHandler;
using Aplication.Services.Validations;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Services
{
    public static class DependencyInjection
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddMediatR(typeof(CreateOrderCommandValidator).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(AddBuyerAggregateWhenOrderStartedDomainEventHandler).GetTypeInfo().Assembly);
        }
    }
}
