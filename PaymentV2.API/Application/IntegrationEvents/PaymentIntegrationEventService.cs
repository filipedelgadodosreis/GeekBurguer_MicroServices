using EventBus.Abstractions;
using EventBus.Events;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System;

namespace PaymentV2.API.Application.IntegrationEvents
{
    public class PaymentIntegrationEventService : IPaymentIntegrationEventService
    {
        private readonly IEventBus _eventBus;
        private readonly ILogger<PaymentIntegrationEventService> _logger;

        public PaymentIntegrationEventService(IEventBus eventBus, ILogger<PaymentIntegrationEventService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        public void AddEvent(IntegrationEvent evt)
        {
            using (LogContext.PushProperty("IntegrationEventContext", $"{evt.Id}-{Program.AppName}"))
            {
                _logger.LogInformation("----- Handling integration event: {IntegrationEventId} : {AppName} - ({@IntegrationEvent})", evt.Id, Program.AppName, evt);

                _eventBus.Publish(evt);
            }
        }
    }
}
