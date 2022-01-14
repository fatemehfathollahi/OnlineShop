using Domain.Core.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplication.Services.Commands
{
    public class ShipOrderCommand : IRequest<bool>
    {

        [DataMember]
        public int OrderNumber { get; private set; }

        public ShipOrderCommand(int orderNumber)
        {
            OrderNumber = orderNumber;
        }

        // Regular CommandHandler
        public class ShipOrderCommandHandler : IRequestHandler<ShipOrderCommand, bool>
        {
            private readonly IOrderRepository _orderRepository;

            public ShipOrderCommandHandler(IOrderRepository orderRepository)
            {
                _orderRepository = orderRepository;
            }

            /// <summary>
            /// Handler which processes the command when
            /// administrator executes ship order from app
            /// </summary>
            /// <param name="command"></param>
            /// <returns></returns>
            public async Task<bool> Handle(ShipOrderCommand command, CancellationToken cancellationToken)
            {
                var orderToUpdate = await _orderRepository.GetAsync(command.OrderNumber);
                if (orderToUpdate == null)
                {
                    return false;
                }

                orderToUpdate.SetShippedStatus();
                return await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            }
        }
    }
}
