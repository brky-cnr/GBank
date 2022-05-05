using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using GBank.Domain.Exceptions;
using GBank.Domain.Interfaces;

namespace GBank.Api.Application.Transactions.Queries
{
    public class GetCustomerTransactionsByDateQueryHandler : IRequestHandler<GetCustomerTransactionsByDateQuery, TransactionDTO>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ITransactionRepository _transactionRepository;
        public GetCustomerTransactionsByDateQueryHandler(ICustomerRepository customerRepository, ITransactionRepository transactionRepository)
        {
            _customerRepository = customerRepository;
            _transactionRepository = transactionRepository;
        }

        public async Task<TransactionDTO> Handle(GetCustomerTransactionsByDateQuery request, CancellationToken cancellationToken)
        {
            var validationResult = new GetCustomerTransactionsByDateQueryValidator().Validate(request);
            if (!validationResult.IsValid)
            {
                var message = string.Join(',', validationResult.Errors.Select(x => x.ErrorMessage).ToList());
                throw new ApiException(message, HttpStatusCode.BadRequest);
            }

            var customer = _customerRepository.GetCustomerAsync(request.CustomerId);
            var customerTransacitons = _transactionRepository.GetCustomerTransactionsByDateAsync(request.CustomerId, request.StartDate, request.EndDate);
            await Task.WhenAll(customerTransacitons, customer);

            if (customer.Result is null)
            {
                throw new ApiException("Customer not found", HttpStatusCode.BadRequest);
            }

            if (customerTransacitons.Result is null || customerTransacitons.Result.Count < 1)
            {
                throw new ApiException("Customer has no transactions", HttpStatusCode.BadRequest);
            }

            return new TransactionDTO
            {
                Transactions = customerTransacitons.Result.Select(x => new TransactionDTOItem
                {
                    AccountId = x.AccountId,
                    CustomerId = x.CustomerId,
                    Amount = x.Amount,
                    Description = x.Description,
                    IsDeposit = x.IsDeposit,
                    CreatedTime = x.CreatedTime,
                }).ToList()
            };
        }
    }
}
