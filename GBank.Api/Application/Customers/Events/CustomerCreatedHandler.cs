using MediatR;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;
using GBank.Domain.Documents;
using GBank.Domain.Events;
using GBank.Domain.Interfaces;

namespace GBank.Api.Application.Customers.Events
{
    public class CustomerCreatedHandler : INotificationHandler<CustomerCreatedEvent>
    {
        private readonly IEventLogRepository _eventLogRepository;
        public CustomerCreatedHandler(IEventLogRepository eventLogRepository)
        {
            _eventLogRepository = eventLogRepository;
        }
        public async Task Handle(CustomerCreatedEvent notification, CancellationToken cancellationToken)
        {
            var eventLog = new EventLog
            {
                Message = $"{notification.CustomerId} customer was created.",
                Data = JsonConvert.SerializeObject(notification.CustomerId)
            };

            await _eventLogRepository.CreateEventLogAsync(eventLog);
        }
    }
}
