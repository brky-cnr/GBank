using MongoDB.Driver;
using System;
using GBank.Domain;
using GBank.Domain.Documents;
using GBank.Domain.Settings;

namespace GBank.Infrastructure
{
    public class GBankContext : IGBankContext
    {
        public IMongoClient MongoClient { get; }
        private readonly string _databaseName;

        public GBankContext(IMongoDBSettings mongoDBSettings)
        {
            MongoClient = new MongoClient(mongoDBSettings.ConnectionString);
            _databaseName = mongoDBSettings.DatabaseName;
        }
        public IMongoCollection<Account> Accounts => MongoClient.GetDatabase(_databaseName, new MongoDatabaseSettings
        {
            ReadConcern = ReadConcern.Majority,
            WriteConcern = new WriteConcern(WriteConcern.WMode.Majority, TimeSpan.FromMilliseconds(1000))
        }).GetCollection<Account>("Accounts");
        public IMongoCollection<Transaction> Transactions => MongoClient.GetDatabase(_databaseName).GetCollection<Transaction>("Transactions");
        public IMongoCollection<Customer> Customers => MongoClient.GetDatabase(_databaseName).GetCollection<Customer>("Customers");
        public IMongoCollection<EventLog> EventLogs => MongoClient.GetDatabase(_databaseName).GetCollection<EventLog>("EventLogs");
    }
}