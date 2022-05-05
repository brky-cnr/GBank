using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using GBank.Domain.Documents;
using GBank.Infrastructure.Settings;

namespace GBank.Infrastructure.Helpers
{
    public static class DBSeed
    {
        public static void Seed(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var settings = serviceScope.ServiceProvider.GetService<IOptions<MongoDBSettings>>().Value;
            var context = new GBankContext(settings);

            var count = context.MongoClient.GetDatabase("GBankDB").GetCollection<Customer>("Customers").CountDocuments(new BsonDocument());
            if (count > 0)
            {
                return;
            }

            var customer = new Customer { Name = "test", Address = "test", CreatedTime = System.DateTime.UtcNow, Email = "test@test.com" };
            context.Customers.InsertOne(customer);
            var account = new Account { CustomerId = customer.Id, Balance = 0, CreatedTime = System.DateTime.UtcNow, AccountTransactions = new() };
            context.Accounts.InsertOne(account);
        }
    }
}