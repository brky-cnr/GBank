using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using GBank.Api.Application.Customers.Commands;
using GBank.Api.Models;

namespace GBank.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/customers")]
    [ProducesResponseType(typeof(HttpResponseBase), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(HttpResponseBase), (int)HttpStatusCode.InternalServerError)]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(HttpResponseBase<string>), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Post([FromBody] RegisterCustomerCommand command)
        {
            var result = await _mediator.Send(command);
            return Created(string.Empty, new HttpResponseBase<string> { Data = result, Code = HttpStatusCode.Created });
        }

    }
}
