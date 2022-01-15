using Aplication.Services.Commands;
using Aplication.Services.Queries;
using Aplication.Services.Validations;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace OnlineShopAPI.Controllers.v1
{
    [ApiVersion("1.0")]
    public class OrderController : BaseApiController
    {
      
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderCommand command)
        {
            CreateOrderCommandValidator validator = new CreateOrderCommandValidator();
            var results = validator.Validate(command);

            if (results.IsValid)
            {
                return Ok(await Mediator.Send(command));
            }

            IList<ValidationFailure> failures = results.Errors;
            return   BadRequest( failures);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new GetAllOrderQuery()));
        }

        [Route("ship")]
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ShipOrderAsync([FromBody] ShipOrderCommand command, [FromHeader(Name = "x-requestid")] string requestId)
        {
            bool commandResult = false;

            if (Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty)
            {

                commandResult = await Mediator.Send(command);
            }

            if (!commandResult)
            {
                return BadRequest();
            }

            return Ok();
        }



    }
}
