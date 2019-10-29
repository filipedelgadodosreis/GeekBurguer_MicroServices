using MongoDB.Bson;
using MongoDB.Driver;
using Ordering.Mongo.Helpers;
using System.Collections.Generic;

namespace Ordering.Mongo.Repositories
{
    public class OrderMongoRepository 
    {
        private readonly MongoHelper _mongoHelper;

        public OrderMongoRepository(MongoHelper mongoHelper)
        {
            _mongoHelper = mongoHelper;
        }

        public string Get(string message)
        {
            var collection = _mongoHelper.MongoDatabase.GetCollection<BsonDocument>("order");
            FilterDefinitionBuilder<BsonDocument> builder = Builders<BsonDocument>.Filter;
            FilterDefinition<BsonDocument> filter = builder.Eq("Message", message);
            BsonDocument bsonDocumentResponse = collection.Find(filter).FirstOrDefault();

            return bsonDocumentResponse == null ? "Registro não encontrado" : bsonDocumentResponse.GetValue("Message").ToString();
        }

        public void Add(Dtos.OrderDto request)
        {
            var collectionChat = _mongoHelper.MongoDatabase.GetCollection<BsonDocument>("order");
            var bsonDocumentRequest = new BsonDocument(
                new Dictionary<string, string> {
                    { "UserId", request. },
                    { "Message", request.Message }
                }
            );

            collectionChat.InsertOne(bsonDocumentRequest);
        }
    }
}
