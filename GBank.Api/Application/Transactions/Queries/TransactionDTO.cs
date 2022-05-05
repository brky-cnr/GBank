using System;
using System.Collections.Generic;

namespace GBank.Api.Application.Transactions.Queries
{
    public class TransactionDTO
    {
        public List<TransactionDTOItem> Transactions { get; set; }
    }

    public class TransactionDTOItem
    {
        public string AccountId { get; set; }
        public string CustomerId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public bool IsDeposit { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
