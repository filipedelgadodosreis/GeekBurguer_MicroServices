using MediatR;
using System;

namespace Payment.Domain.Events
{
    public class OrderStatusChangedToPaidDomainEvent : INotification
    {
        public Guid OrderId { get; }

        public OrderStatusChangedToPaidDomainEvent(Guid orderId)
        {
            OrderId = orderId;
        }
    }
}
