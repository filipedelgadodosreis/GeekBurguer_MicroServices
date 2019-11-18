using EventBus.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using Payment.Domain.Events;
using PaymentV2.API.Application.IntegrationEvents;
using Serilog.Context;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentV2.API.Application.DomainEventHandlers.OrderPaid
{
    public class OrderStatusChangedToPaidDomainEventHandler : INotificationHandler<OrderStatusChangedToPaidDomainEvent>
    {
        private readonly IEventBus _eventBus;
        private readonly ILogger<OrderStatusChangedToPaidDomainEventHandler> _logger;

        public OrderStatusChangedToPaidDomainEventHandler(IEventBus eventBus, ILogger<OrderStatusChangedToPaidDomainEventHandler> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        public async Task Handle(OrderStatusChangedToPaidDomainEvent notification, CancellationToken cancellationToken)
        {
            using (LogContext.PushProperty("IntegrationEventContext", $"{notification.OrderId}-{Program.AppName}"))
            {
                _logger.LogInformation("----- Handling integration event: {IntegrationEventId} : {AppName} - ({@IntegrationEvent})", notification.OrderId, Program.AppName, notification);

                var orderStatusChangedToPaidIntegrationEvent = new OrderStatusChangedToPaidIntegrationEvent(notification.OrderId);

                _eventBus.Publish(orderStatusChangedToPaidIntegrationEvent);
            }
        }
    }
}
