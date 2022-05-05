using MediatR;
using GBank.Domain.Documents;
using GBank.Domain.Events;
using GBank.Domain.Exceptions;
using GBank.Domain.Interfaces;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace GBank.Api.Application.Transactions.Commands
{
    public class PlaceAccountTransactionCommandHandler : IRequestHandler<PlaceAccountTransactionCommand, string>
    {
        private readonly IMediator _mediator;
        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionRepository _transactionRepository;

        public PlaceAccountTransactionCommandHandler(IMediator mediator, IAccountRepository accountRepository, ITransactionRepository transactionRepository)
        {
            _mediator = mediator;
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
        }

        public async Task<string> Handle(PlaceAccountTransactionCommand request, CancellationToken cancellationToken)
        {
            var validationResult = new PlaceAccountTransactionCommandValidator().Validate(request);
            if (!validationResult.IsValid)
            {
                var message = string.Join(',', validationResult.Errors.Select(x => x.ErrorMessage).ToList());
                throw new ApiException(message, HttpStatusCode.BadRequest);
            }

            var account = await _accountRepository.GetAccountAsync(request.AccountId, request.CustomerId);
            if (account is null)
            {
                throw new ApiException("Account not found", HttpStatusCode.BadRequest);
            }

            var accountTransaction = new Transaction()
            {
                AccountId = request.AccountId,
                CustomerId = request.CustomerId,
                Amount = request.Amount,
                Description = request.Description,
                IsDeposit = request.IsDeposit
            };

            var result = await _transactionRepository.CreateAccountTransactionAsync(accountTransaction);

            _ = _mediator.Publish(new AccountTransactionPlacedEvent
            {
                TransactionId = result.Id
            });

            return result.Id;
        }
    }
}
