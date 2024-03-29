﻿using Dapper;
using EventBus.Abstractions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Payment.BackgroundTasks.Configuration;
using Payment.BackgroundTasks.IntegrationEvents;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
            _logger.LogDebug("ReceivePaymentService iniciando.");

            stoppingToken.Register(() => _logger.LogDebug("#1 ReceivePaymentService background task pausando."));

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogDebug("ReceivePaymentService background task em execução.");

                SendPayment();

                await Task.Delay(100, stoppingToken);
            }

            _logger.LogDebug("ReceivePaymentService background task pausando.");

            await Task.CompletedTask;
        }

        private void SendPayment()
        {
            _logger.LogDebug("Checando novas ordens para processo de pagamento.");

            var orderIds = GetConfirmedOrders();

            foreach (var orderId in orderIds)
            {
                var orderPaymentIntegrationEvent = new OrderPaymentIntegrationEvent(Guid.NewGuid());

                _logger.LogInformation("----- Publishing integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent})", orderPaymentSuccededIntegrationEvent.OrderId, Program.AppName, orderPaymentSuccededIntegrationEvent);

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
