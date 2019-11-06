using EventBus.Abstractions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Payment.BackgroundTasks.Configuration;
using Payment.BackgroundTasks.IntegrationEvents;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Payment.BackgroundTasks.Tasks
{
    public class ReceivePaymentService : BackgroundService
    {
        private readonly IEventBus _eventBus;
        private readonly BackgroundTaskSettings _settings;
        private readonly ILogger<ReceivePaymentService> _logger;


        public ReceivePaymentService(ILogger<ReceivePaymentService> logger, IOptions<BackgroundTaskSettings> settings, IEventBus eventBus)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug("ReceivePaymentService is starting.");

            stoppingToken.Register(() => _logger.LogDebug("#1 ReceivePaymentService background task is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogDebug("ReceivePaymentService background task is doing background work.");

                SendPayment();

                await Task.Delay(100, stoppingToken);
            }
                
            _logger.LogDebug("ReceivePaymentService background task is stopping.");

            await Task.CompletedTask;
        }

        private void SendPayment()
        {
            var orderPaymentSuccededIntegrationEvent = new OrderPaymentSuccededIntegrationEvent(Guid.NewGuid());

            _eventBus.Publish(orderPaymentSuccededIntegrationEvent);
        }
    }
}
