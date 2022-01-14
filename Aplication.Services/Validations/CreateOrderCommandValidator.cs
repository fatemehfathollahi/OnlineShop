using Aplication.Services.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Aplication.Services.Commands.CreateOrderCommand;

namespace Aplication.Services.Validations
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(command => command.City).NotEmpty();
            RuleFor(command => command.Street).NotEmpty();
            RuleFor(command => command.State).NotEmpty();
            RuleFor(command => command.Country).NotEmpty();
            RuleFor(command => command.ZipCode).NotEmpty();
           // RuleFor(command => command.UserId).NotEmpty().Must(BeValidOnlyOrder).WithMessage("Each order must be for one person");
            RuleFor(command => command.OrderDate).NotEmpty().Must(BeValidOrderingDate).WithMessage("It is not possible to order at this time");
            RuleFor(command => command.OrderItems).Must(ContainOrderItems).WithMessage("No order items found");
        }

        private bool BeValidOnlyOrder(string userId)
        {
            return userId.Any();
        }
        private bool BeValidOrderingDate(DateTime dateTime)
        {
            TimeSpan time = DateTime.Now.TimeOfDay;
            return (time < new TimeSpan(08, 00, 00) || time > new TimeSpan(19, 00, 00));
        }

        private bool ContainOrderItems(IEnumerable<OrderItemDTO> orderItems)
        {
            return orderItems.Any();
        }
    }
}
