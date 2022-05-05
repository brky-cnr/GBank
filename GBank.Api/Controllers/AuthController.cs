using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using GBank.Api.Application.Token;
using GBank.Api.Models;

namespace GBank.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("token")]
        [ProducesResponseType(typeof(HttpResponseBase<string>), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Post(TokenCommand command)
        {
            var result = await _mediator.Send(command);
            return Created(string.Empty, new HttpResponseBase<string> { Data = result, Code = HttpStatusCode.Created });
        }
    }
}