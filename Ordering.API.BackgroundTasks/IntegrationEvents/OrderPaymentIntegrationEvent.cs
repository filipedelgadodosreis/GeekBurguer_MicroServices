using System;
using EventBus.Events;


namespace Ordering.API.BackgroundTasks.IntegrationEvents
{
    public class OrderPaymentIntegrationEvent : IntegrationEvent
    {
        public Guid OrderId { get; }

        public OrderPaymentIntegrationEvent(Guid orderId) => OrderId = orderId;
    }
}
