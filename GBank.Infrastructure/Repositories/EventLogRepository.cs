using System.Threading.Tasks;
using GBank.Domain;
using GBank.Domain.Documents;
using GBank.Domain.Interfaces;
using GBank.Domain.Settings;

namespace GBank.Infrastructure.Repositories
{
    public class EventLogRepository : IEventLogRepository
    {
        private readonly IGBankContext _context;
        public EventLogRepository(IMongoDBSettings mongoDBSettings)
        {
            _context = new GBankContext(mongoDBSettings);
        }

        public async Task CreateEventLogAsync(EventLog eventLog)
        {
            await _context.EventLogs.InsertOneAsync(eventLog);
        }
    }
}
