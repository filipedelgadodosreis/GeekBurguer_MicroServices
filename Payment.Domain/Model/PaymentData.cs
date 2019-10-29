using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Payment.Domain.Model
{
    public class PaymentData
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public Guid PaymentId { get; set; }

        [BsonElement("OrderId")]
        public Guid OrderId { get; private set; }

        [BsonElement("StoreId")]
        public Guid StoreId { get; private set; }

        public Guid RequesterId { get; private set; }

        public string PayType { get; private set; }

        public string CardNumber { get; private set; }

        public string SecurityCode { get; private set; }

        public string CardOwnerName { get; private set; }

        public DateTime ExpirationDate { get; private set; }
    }
}
