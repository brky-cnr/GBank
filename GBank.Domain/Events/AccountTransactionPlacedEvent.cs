using MediatR;
using GBank.Domain.Documents;

namespace GBank.Domain.Events
{
    public class AccountTransactionPlacedEvent : INotification
    {
        public string TransactionId { get; set; }
    }
}