using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Mongo.Helpers
{
    public class MongoHelper
    {
        private readonly IOptions<Dtos.ConnectionStringDto> _connectionString;
        public IMongoDatabase MongoDatabase;

        public MongoHelper(IOptions<Dtos.ConnectionStringDto> connectionString)
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
