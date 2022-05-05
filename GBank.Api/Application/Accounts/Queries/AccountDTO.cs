using System;
using System.Collections.Generic;

namespace GBank.Api.Application.Accounts.Queries
{
    public class AccountDTO
    {
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public decimal Balance { get; set; }
        public List<AccountTransactionDTO> AccountTransactions { get; set; }
    }
    public class AccountTransactionDTO
    {
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public bool IsDeposit { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}