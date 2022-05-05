namespace GBank.Domain.Documents
{
    public class Transaction : Document
    {
        public string CustomerId { get; set; }
        public string AccountId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public bool IsDeposit { get; set; }
    }
}