using System;

namespace GeekBurger.Payment.Contract
{
    public class Payment
    {
        public Guid OrderId { get; private set; }

        public Guid StoreId { get; private set; }

        public string PayType { get; private set; }

        public Guid RequesterId { get; private set; }

        public string CardNumber { get; private set; }

        public string SecurityCode { get; private set; }

        public string CardOwnerName { get; private set; }

        public DateTime ExpirationDate { get; private set; }
    }
}
