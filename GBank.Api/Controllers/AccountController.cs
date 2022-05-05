using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using GBank.Api.Application.Accounts.Commands;
using GBank.Api.Application.Accounts.Queries;
using GBank.Api.Models;

namespace GBank.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/accounts")]
    [ProducesResponseType(typeof(HttpResponseBase), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(HttpResponseBase), (int)HttpStatusCode.InternalServerError)]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{accountId}")]
        [ProducesResponseType(typeof(HttpResponseBase<AccountDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get([FromQuery] GetAccountQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(new HttpResponseBase<AccountDTO> { Data = result, Code = HttpStatusCode.OK });
        }

        [HttpPost]
        [ProducesResponseType(typeof(HttpResponseBase<string>), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Post([FromBody] RegisterAccountCommand command)
        {
            var result = await _mediator.Send(command);
            return Created(string.Empty, new HttpResponseBase<string> { Data = result, Code = HttpStatusCode.Created });
        }
    }
}
