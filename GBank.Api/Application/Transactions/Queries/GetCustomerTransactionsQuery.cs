using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GBank.Api.Application.Transactions.Queries
{
    public class GetCustomerTransactionsQuery : IRequest<TransactionDTO>
    {
        [FromRoute(Name = "customerId")]
        public string CustomerId { get; set; }


    }
}
