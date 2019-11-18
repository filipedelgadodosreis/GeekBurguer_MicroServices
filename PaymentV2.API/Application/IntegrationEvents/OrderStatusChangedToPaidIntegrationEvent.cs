using EventBus.Events;
using System;

namespace PaymentV2.API.Application.IntegrationEvents
{
    public class OrderStatusChangedToPaidIntegrationEvent : IntegrationEvent
    {
        public Guid OrderId { get; }

        public OrderStatusChangedToPaidIntegrationEvent(Guid orderId)
        {
            OrderId = orderId;
        }
    }
}
