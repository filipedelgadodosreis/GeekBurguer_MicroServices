using System;

namespace GeekBurger.Ordering.Contract
{
    public class OrderChanged
    {
        public Guid OrderId { get; private set; }

        public Guid StoreId { get; private set; }

        public PaymentState State { get; private set; }
    }

    public enum PaymentState
    {
        Paid,
        Canceled,
        Finished
    }
}
