using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using GBank.Domain.Documents;
using GBank.Domain.Events;
using GBank.Domain.Interfaces;

namespace GBank.Api.Application.Transactions.Events
{
    public class AccountTransactionPlacedHandler : INotificationHandler<AccountTransactionPlacedEvent>
    {
        private readonly IEventLogRepository _eventLogRepository;
        public AccountTransactionPlacedHandler(IEventLogRepository eventLogRepository)
        {
            _eventLogRepository = eventLogRepository;
        }
        public async Task Handle(AccountTransactionPlacedEvent notification, CancellationToken cancellationToken)
        {
            var eventLog = new EventLog
            {
                Message = $"{notification.TransactionId} transaction was placed.",
                Data = JsonConvert.SerializeObject(notification)
            };

            await _eventLogRepository.CreateEventLogAsync(eventLog);
        }
    }
}
