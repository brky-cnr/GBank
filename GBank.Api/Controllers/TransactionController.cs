using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using GBank.Api.Application.Transactions.Commands;
using GBank.Api.Application.Transactions.Queries;
using GBank.Api.Models;

namespace GBank.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/transactions")]
    [ProducesResponseType(typeof(HttpResponseBase), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(HttpResponseBase), (int)HttpStatusCode.InternalServerError)]
    public class TransactionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TransactionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("customers/{customerId}")]
        [ProducesResponseType(typeof(HttpResponseBase<TransactionDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get([FromQuery] GetCustomerTransactionsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(new HttpResponseBase<TransactionDTO> { Data = result, Code = HttpStatusCode.OK });
        }

        [HttpGet("customers/{customerId}/{startDate}/{endDate}")]
        [ProducesResponseType(typeof(HttpResponseBase<TransactionDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCustomerTransactions([FromQuery] GetCustomerTransactionsByDateQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(new HttpResponseBase<TransactionDTO> { Data = result, Code = HttpStatusCode.OK });
        }

        [HttpPost]
        [ProducesResponseType(typeof(HttpResponseBase<string>), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Post([FromBody] PlaceAccountTransactionCommand command)
        {
            var result = await _mediator.Send(command);
            return Created(string.Empty, new HttpResponseBase<string> { Data = result, Code = HttpStatusCode.Created });
        }



    }
}
