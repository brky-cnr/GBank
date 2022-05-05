using GBank.Domain.Documents;
using System.Threading.Tasks;

namespace GBank.Domain.Interfaces
{
    public interface IAccountRepository
    {
        Task<string> CreateAccountAsync(Account account);
        Task<Account> GetAccountAsync(string id);
        Task<Account> GetAccountAsync(string id, string customerId);
    }
}