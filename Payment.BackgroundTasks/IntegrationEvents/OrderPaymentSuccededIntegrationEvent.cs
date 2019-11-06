using EventBus.Events;
using System;

namespace Payment.BackgroundTasks.IntegrationEvents
{
    public class OrderPaymentSuccededIntegrationEvent : IntegrationEvent
    {
        public Guid OrderId { get; }

        public OrderPaymentSuccededIntegrationEvent(Guid orderId) => OrderId = orderId;
    }
}
