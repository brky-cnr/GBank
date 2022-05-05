using System.Threading.Tasks;
using MongoDB.Driver;
using GBank.Domain;
using GBank.Domain.Documents;
using GBank.Domain.Interfaces;
using GBank.Domain.Settings;

namespace GBank.Infrastructure.Repositories
{
    public class AccoutRepository : IAccountRepository
    {
        private readonly IGBankContext _context;
        public AccoutRepository(IMongoDBSettings mongoDBSettings)
        {
            _context = new GBankContext(mongoDBSettings);
        }

        public async Task<string> CreateAccountAsync(Account account)
        {
            await _context.Accounts.InsertOneAsync(account);
            return account.Id.ToString();
        }

        public async Task<Account> GetAccountAsync(string id)
        {
            var filter = Builders<Account>.Filter.Eq(x => x.Id, id);
            var account = await _context.Accounts.Find(filter).FirstOrDefaultAsync();
            return account;
        }

        public async Task<Account> GetAccountAsync(string id, string customerId)
        {
            var filter = Builders<Account>.Filter.Eq(x => x.Id, id) & Builders<Account>.Filter.Eq(x => x.CustomerId, customerId);
            var account = await _context.Accounts.Find(filter).FirstOrDefaultAsync();
            return account;
        }
    }
}
