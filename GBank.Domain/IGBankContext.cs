using MongoDB.Driver;
using GBank.Domain.Documents;

namespace GBank.Domain
{
    public interface IGBankContext
    {
        IMongoClient MongoClient { get; }
        IMongoCollection<Customer> Customers { get; }
        IMongoCollection<Account> Accounts { get; }
        IMongoCollection<Transaction> Transactions { get; }
        IMongoCollection<EventLog> EventLogs { get; }
    }
}
