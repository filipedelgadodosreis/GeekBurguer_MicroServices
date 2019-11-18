using System;

namespace Payment.Domain.Model
{
    public class Payment
    {
        public int PaymentId { get; set; }

        public Guid OrderId { get; private set; }

        public Guid StoreId { get; private set; }

        public Guid RequesterId { get; private set; }

        public string PayType { get; private set; }

        public string CardNumber { get; private set; }

        public string SecurityCode { get; private set; }

        public string CardOwnerName { get; private set; }

        public DateTime ExpirationDate { get; private set; }
    }
}
