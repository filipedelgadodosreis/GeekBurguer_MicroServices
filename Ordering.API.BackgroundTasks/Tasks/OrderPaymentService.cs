using Dapper;
using EventBus.Abstractions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ordering.API.BackgroundTasks.Configuration;
using Ordering.API.BackgroundTasks.IntegrationEvents;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.API.BackgroundTasks.Tasks
{
    public class OrderPaymentService : BackgroundService
    {
        private readonly IEventBus _eventBus;
        private readonly BackgroundTaskSettings _settings;
        private readonly ILogger<OrderPaymentService> _logger;

        public OrderPaymentService(ILogger<OrderPaymentService> logger, IOptions<BackgroundTaskSettings> settings, IEventBus eventBus)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug("OrderPaymentService iniciando.");

            stoppingToken.Register(() => _logger.LogDebug("#1 OrderPaymentService background task pausando."));

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogDebug("OrderPaymentService background task em execução.");

                SendPayment();

                await Task.Delay(_settings.CheckUpdateTime, stoppingToken);
            }

            _logger.LogDebug("OrderPaymentService background task pausando.");

            await Task.CompletedTask;
        }

        private void SendPayment()
        {
            _logger.LogDebug("Checando novas ordens para processo de pagamento.");

            var orderIds = GetConfirmedOrders();

            foreach (var orderId in orderIds)
            {
                var orderPaymentIntegrationEvent = new OrderPaymentIntegrationEvent(Guid.NewGuid());

                _logger.LogInformation("----- Publishing integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent})", orderPaymentIntegrationEvent.OrderId, Program.AppName, orderPaymentIntegrationEvent);

                _eventBus.Publish(orderPaymentIntegrationEvent);
            }

            _logger.LogDebug("Ordens processadas com sucesso!");
        }

        private IEnumerable<Guid> GetConfirmedOrders()
        {
            IEnumerable<Guid> orderIds = new List<Guid>();

            using (var conn = new SqlConnection(_settings.ConnectionString))
            {
                try
                {
                    conn.Open();
                    orderIds = conn.Query<Guid>(
                        @"SELECT OrderId FROM Order  
                            WHERE DATEDIFF(minute, [OrderDate], GETDATE()) >= @PeriodTime
                            AND [OrderStatusId] = 1",
                        new { _settings.PeriodTime });
                }
                catch (SqlException exception)
                {
                    _logger.LogError(exception, "ERROR: Não foi possível conexão com a base de dados: {Message}", exception.Message);
                }
            }

            return orderIds;
        }
    }
}
