using System.Collections.Generic;

namespace GBank.Domain.Documents
{
    public class Account : Document
    {
        public string CustomerId { get; set; }
        public decimal Balance { get; set; }
        public List<AccountTransaction> AccountTransactions { get; set; }
    }
}