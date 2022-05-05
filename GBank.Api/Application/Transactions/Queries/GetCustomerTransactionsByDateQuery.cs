using System;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace GBank.Api.Application.Transactions.Queries
{
    public class GetCustomerTransactionsByDateQuery : IRequest<TransactionDTO>
    {
        [FromRoute(Name = "customerId")]
        public string CustomerId { get; set; }

        [FromRoute(Name = "startDate")]
        public DateTime StartDate { get; set; }

        [FromRoute(Name = "endDate")]
        public DateTime EndDate { get; set; }
    }
}
