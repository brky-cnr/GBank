using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using GBank.Domain.Exceptions;
using GBank.Domain.Interfaces;

namespace GBank.Api.Application.Accounts.Queries
{
    public class GetAccountQueryHandler : IRequestHandler<GetAccountQuery, AccountDTO>
    {
        private readonly IAccountRepository _accountRepository;
        public GetAccountQueryHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<AccountDTO> Handle(GetAccountQuery request, CancellationToken cancellationToken)
        {
            var validationResult = new GetAccountQueryValidator().Validate(request);
            if (!validationResult.IsValid)
            {
                var message = string.Join(',', validationResult.Errors.Select(x => x.ErrorMessage).ToList());
                throw new ApiException(message, HttpStatusCode.BadRequest);
            }

            var account = await _accountRepository.GetAccountAsync(request.AccountId.ToString());
            if (account is null)
            {
                throw new ApiException("Account not found", System.Net.HttpStatusCode.BadRequest);
            }

            return new AccountDTO
            {
                Id = account.Id,
                CustomerId = account.CustomerId,
                Balance = account.Balance,
                AccountTransactions = account.AccountTransactions.Select(x => new AccountTransactionDTO
                {
                    Amount = x.Amount,
                    Description = x.Description,
                    IsDeposit = x.IsDeposit,
                    CreatedTime = x.CreatedTime
                }).ToList()
            };

        }
    }
}
