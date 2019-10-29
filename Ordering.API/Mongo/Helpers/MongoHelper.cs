using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Ordering.API.Mongo.Dtos;

namespace Ordering.API.Mongo.Helpers
{
    public class MongoHelper
    {
        private readonly IOptions<ConnectionStringDto> _connectionString;
        public IMongoDatabase MongoDatabase;

        public MongoHelper(IOptions<ConnectionStringDto> connectionString)
        {
            _connectionString = connectionString;
            CreateMongoDatabase();
        }

        private void CreateMongoDatabase()
        {
            MongoClient client = new MongoClient(_connectionString.Value.MongoDB);
            MongoDatabase = client.GetDatabase("fiap");
        }
    }
}
