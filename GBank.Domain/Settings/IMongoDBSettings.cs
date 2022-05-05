namespace GBank.Domain.Settings
{
    public interface IMongoDBSettings
    {
        string DatabaseName { get; set; }
        string ConnectionString { get; set; }
    }
}