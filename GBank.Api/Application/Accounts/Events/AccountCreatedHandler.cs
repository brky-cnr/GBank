using MediatR;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;
using GBank.Domain.Documents;
using GBank.Domain.Events;
using GBank.Domain.Interfaces;

namespace GBank.Api.Application.Accounts.Events
{
    public class AccountCreatedHandler : INotificationHandler<AccountCreatedEvent>
    {
        private readonly IEventLogRepository _eventLogRepository;
        public AccountCreatedHandler(IEventLogRepository eventLogRepository)
        {
            _eventLogRepository = eventLogRepository;
        }
        public async Task Handle(AccountCreatedEvent notification, CancellationToken cancellationToken)
        {
            var eventLog = new EventLog
            {
                Message = $"{notification.AccountId} account was created.",
                Data = JsonConvert.SerializeObject(notification.AccountId)
            };

            await _eventLogRepository.CreateEventLogAsync(eventLog);
        }
    }
}
