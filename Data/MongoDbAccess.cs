using MongoDB.Bson;
using MongoDB.Driver;
using System.Security.Authentication;

namespace Assignment8.Data
{
    public class MongoDbAccess
    {
        private readonly MongoClient _client;

        public MongoDbAccess(IConfiguration configuration)
        {
            var connectionUri = configuration.GetConnectionString("MongoDb");

            if (string.IsNullOrEmpty(connectionUri))
            {
                throw new InvalidOperationException("MongoDB connection string is not configured.");
            }

            var settings = MongoClientSettings.FromConnectionString(connectionUri);
            settings.SslSettings = new SslSettings { EnabledSslProtocols = SslProtocols.Tls12 };
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);

            _client = new MongoClient(settings);
        }

        public IMongoDatabase GetDatabase(string databaseName)
        {
            if (string.IsNullOrEmpty(databaseName))
            {
                throw new ArgumentException("Database name cannot be null or empty.", nameof(databaseName));
            }

            return _client.GetDatabase(databaseName);
        }

        public bool TestConnection()
        {
            try
            {
                var result = _client.GetDatabase("admin").RunCommand<BsonDocument>(new BsonDocument("ping", 1));
                Console.WriteLine("Pinged your deployment. You successfully connected to MongoDB!");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }
    }
}
