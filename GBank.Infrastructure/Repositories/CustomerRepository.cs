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
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IGBankContext _context;
        public CustomerRepository(IMongoDBSettings mongoDBSettings)
        {
            _context = new GBankContext(mongoDBSettings);
        }

        public async Task<string> CreateCustomerAsync(Customer customer)
        {
            var registeredCustomer = await GetCustomerByEmailAsync(customer.Email);
            if (registeredCustomer is not null)
            {
                throw new ApiException("You have already account!", System.Net.HttpStatusCode.BadRequest);
            }

            await _context.Customers.InsertOneAsync(customer);
            return customer.Id.ToString();
        }

        public async Task<Customer> GetCustomerAsync(string id)
        {
            var filter = Builders<Customer>.Filter.Eq(x => x.Id, id);
            var customer = await _context.Customers.Find(filter).FirstOrDefaultAsync();
            return customer;
        }

        private async Task<Customer> GetCustomerByEmailAsync(string email)
        {
            var filter = Builders<Customer>.Filter.Eq(x => x.Email, email);
            var customer = await _context.Customers.Find(filter).FirstOrDefaultAsync();
            return customer;
        }
    }
}
