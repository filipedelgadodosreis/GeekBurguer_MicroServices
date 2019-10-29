using GeekBurger.Ordering.Contract;
using MongoDB.Bson;
using Newtonsoft.Json;
using System.Collections.Generic;
using Ordering.API.Mongo.Helpers;

namespace Ordering.API.Mongo.Repositories
{
    public class OrderMongoRepository
    {
        private readonly MongoHelper _mongoHelper;

        public OrderMongoRepository(MongoHelper mongoHelper)
        {
            _mongoHelper = mongoHelper;
        }

        public void Add(Order request)
        {
            var collection = _mongoHelper.MongoDatabase.GetCollection<BsonDocument>("order");
            var bsonDocumentRequest = new BsonDocument(
                new Dictionary<string, string> {
                    { "OrderId", request.OrderId.ToString() },
                    { "StoreId", request.StoreId.ToString() },
                    { "Total", request.Total },
                    { "Products", JsonConvert.SerializeObject(request.Products) }
                }
            );

            collection.InsertOne(bsonDocumentRequest);
        }
    }
}
