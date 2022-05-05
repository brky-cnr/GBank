using MediatR;
using GBank.Domain.Documents;

namespace GBank.Domain.Events
{
    public class AccountCreatedEvent : INotification
    {
        public string AccountId { get; set; }
    }
}