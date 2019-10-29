using MongoDB.Driver;
using Payment.Domain.Model;
using Microsoft.Extensions.Options;

namespace Payment.Infrastructure
{
    public class PaymentDataContext
    {
        private readonly IMongoDatabase _database = null;

        public PaymentDataContext(IOptions<PaymentDatabaseSettings> settings)
        {
            var client = new MongoClient(settings.Value.MongoConnectionString);

            _database = client.GetDatabase(settings.Value.MongoDatabase);
        }

        public IMongoCollection<PaymentData> PaymentData
        {
            get
            {
                return _database.GetCollection<PaymentData>("PaymentDataModel");
            }
        }
    }
}
