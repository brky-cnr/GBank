using MediatR;

namespace GBank.Domain.Events
{
    public class CustomerCreatedEvent : INotification
    {
        public string CustomerId { get; set; }
    }
}