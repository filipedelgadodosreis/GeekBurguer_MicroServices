using EventBus.Events;
using System;

namespace Payment.BackgroundTasks.IntegrationEvents
{
    public class OrderPaymentIntegrationEvent : IntegrationEvent
    {
        public Guid OrderId { get; }

        public OrderPaymentIntegrationEvent(Guid orderId) => OrderId = orderId;
    }
}
