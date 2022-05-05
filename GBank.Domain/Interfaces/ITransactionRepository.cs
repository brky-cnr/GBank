using GBank.Domain.Documents;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GBank.Domain.Interfaces
{
    public interface ITransactionRepository
    {
        Task<List<Transaction>> GetCustomerTransactionsAsync(string customerId);
        Task<List<Transaction>> GetCustomerTransactionsByDateAsync(string id, DateTime startDate, DateTime endDate);
        Task<Transaction> CreateAccountTransactionAsync(Transaction transaction);
    }
}