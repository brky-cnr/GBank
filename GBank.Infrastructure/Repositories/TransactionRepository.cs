using System.Threading.Tasks;
using MongoDB.Driver;
using GBank.Domain;
using GBank.Domain.Documents;
using GBank.Domain.Exceptions;
using GBank.Domain.Interfaces;
using GBank.Domain.Settings;
using System.Collections.Generic;
using System;

namespace GBank.Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly IGBankContext _context;
        public TransactionRepository(IMongoDBSettings mongoDBSettings)
        {
            _context = new GBankContext(mongoDBSettings);
        }

        public async Task<List<Transaction>> GetCustomerTransactionsAsync(string customerId)
        {
            var filter = Builders<Transaction>.Filter.Eq(x => x.CustomerId, customerId);
            var transactions = await _context.Transactions.Find(filter).ToListAsync();
            return transactions;
        }

        public async Task<Transaction> CreateAccountTransactionAsync(Transaction accountTransaction)
        {
            using (var session = await _context.MongoClient.StartSessionAsync(new ClientSessionOptions { CausalConsistency = true }))
            {
                var filter = Builders<Account>.Filter.Eq(x => x.Id, accountTransaction.AccountId);
                var account = await _context.Accounts.Find(filter).FirstOrDefaultAsync();
                if (accountTransaction.IsDeposit)
                {
                    account.Balance += accountTransaction.Amount;
                }
                else
                {
                    if (account.Balance < accountTransaction.Amount)
                    {
                        throw new ApiException("Insufficient balance :(", System.Net.HttpStatusCode.BadRequest);
                    }
                    account.Balance -= accountTransaction.Amount;
                }
                await _context.Transactions.InsertOneAsync(accountTransaction);
                UpdateDefinition<Account> updateDefinition = Builders<Account>.Update.Set(x => x.Balance, account.Balance)
                                                                                    .Push(x => x.AccountTransactions, new AccountTransaction
                                                                                    {
                                                                                        Id = accountTransaction.Id,
                                                                                        Amount = accountTransaction.Amount,
                                                                                        Description = accountTransaction.Description,
                                                                                        IsDeposit = accountTransaction.IsDeposit,
                                                                                        CreatedTime = accountTransaction.CreatedTime
                                                                                    });
                _context.Accounts.UpdateOne(filter, updateDefinition);
                return accountTransaction;
            }
        }

        public async Task<List<Transaction>> GetCustomerTransactionsByDateAsync(string id, DateTime startDate, DateTime endDate)
        {
            var filter = Builders<Transaction>.Filter.Eq(x => x.CustomerId, id) &
                         Builders<Transaction>.Filter.Gte(x => x.CreatedTime, startDate) &
                         Builders<Transaction>.Filter.Lte(x => x.CreatedTime, endDate);
            var transactions = await _context.Transactions.Find(filter).ToListAsync();
            return transactions;
        }
    }
}
