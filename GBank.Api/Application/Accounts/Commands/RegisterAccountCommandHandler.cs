using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using GBank.Domain.Interfaces;
using GBank.Domain.Events;
using GBank.Domain.Exceptions;
using GBank.Domain.Documents;
namespace GBank.Api.Application.Accounts.Commands
{
    public class RegisterAccountCommandHandler : IRequestHandler<RegisterAccountCommand, string>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IMediator _mediator;
        public RegisterAccountCommandHandler(ICustomerRepository customerRepository, IAccountRepository accountRepository, IMediator mediator)
        {
            _customerRepository = customerRepository;
            _accountRepository = accountRepository;
            _mediator = mediator;
        }

        public async Task<string> Handle(RegisterAccountCommand request, CancellationToken cancellationToken)
        {
            var validationResult = new RegisterAccountCommandValidator().Validate(request);
            if (!validationResult.IsValid)
            {
                var message = string.Join(',', validationResult.Errors.Select(x => x.ErrorMessage).ToList());
                throw new ApiException(message, HttpStatusCode.BadRequest);
            }

            var customer = await _customerRepository.GetCustomerAsync(request.CustomerId);
            if (customer is null)
            {
                throw new ApiException("Customer not found!", HttpStatusCode.BadRequest);
            }

            var account = new Account
            {
                CustomerId = request.CustomerId,
                Balance = request.Balance,
                AccountTransactions = new()
            };

            var accountId = await _accountRepository.CreateAccountAsync(account);

            _ = _mediator.Publish(new AccountCreatedEvent
            {
                AccountId = accountId
            }, cancellationToken);

            return accountId;
        }
    }
}
