using System;

namespace ConsoleAppTestTopic.Events
{
    public class OrderPaymentSuccededIntegrationEvent : IntegrationEvent
    {
        public Guid OrderId { get; }

        public OrderPaymentSuccededIntegrationEvent(Guid orderId) => OrderId = orderId;
    }
}
