using GeekBurger.UI.Contract;
using MongoDB.Bson;
using Newtonsoft.Json;
using Ordering.API.Mongo.Helpers;
using System.Collections.Generic;

namespace Ordering.API.Mongo.Repositories
{
    public class OrderMongoRepository
    {
        private readonly MongoHelper _mongoHelper;

        public OrderMongoRepository(MongoHelper mongoHelper)
        {
            _mongoHelper = mongoHelper;
        }

        public void Add(NewOrderMessage request)
        {
            var collection = _mongoHelper.MongoDatabase.GetCollection<BsonDocument>("order");
            var bsonDocumentRequest = new BsonDocument(
                new Dictionary<string, string> {
                    { "OrderId", request.OrderId.ToString() },
                    { "StoreId", request.StoreId.ToString() },
                    { "Products", JsonConvert.SerializeObject(request.Products) }
                }
            );

            collection.InsertOne(bsonDocumentRequest);
        }
    }
}
