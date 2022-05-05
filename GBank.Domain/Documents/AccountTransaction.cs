using System;

namespace GBank.Domain.Documents
{
    public class AccountTransaction
    {
        public string Id { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public bool IsDeposit { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}