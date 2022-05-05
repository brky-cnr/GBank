using GBank.Domain.Settings;

namespace GBank.Infrastructure.Settings
{
    public class MongoDBSettings : IMongoDBSettings
    {
        public string DatabaseName { get; set; }
        public string ConnectionString { get; set; }
    }
}