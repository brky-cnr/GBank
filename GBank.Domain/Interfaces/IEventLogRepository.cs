using GBank.Domain.Documents;
using System.Threading.Tasks;

namespace GBank.Domain.Interfaces
{
    public interface IEventLogRepository
    {
        Task CreateEventLogAsync(EventLog eventLog);
    }
}