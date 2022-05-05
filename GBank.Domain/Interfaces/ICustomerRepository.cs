using System.Threading.Tasks;
using GBank.Domain.Documents;

namespace GBank.Domain.Interfaces
{
    public interface ICustomerRepository
    {
        Task<string> CreateCustomerAsync(Customer customer);
        Task<Customer> GetCustomerAsync(string id);
    }
}