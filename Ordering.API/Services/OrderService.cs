using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Ordering.API.Sql.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;
using GeekBurger.UI.Contract;
using Newtonsoft.Json;
using System.Text;

namespace Ordering.API.Services
{
    public class OrderService : IOrderService
    {
        private readonly IConfiguration _configuration;
        private readonly OrderSqlRepository _orderSqlRepository;

        public OrderService(IConfiguration configuration, OrderSqlRepository orderSqlRepository)
        {
            _configuration = configuration;
            _orderSqlRepository = orderSqlRepository;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            GetOrder();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private async void GetOrder()
        {
            string connectionString = _configuration["serviceBus:connectionString"];
            var subscriptionClient = new SubscriptionClient(connectionString, "newOrder", "minhaSubscrition");
            await subscriptionClient.RemoveRuleAsync("$Default");
            await subscriptionClient.AddRuleAsync(new RuleDescription
            {
                Filter = new CorrelationFilter
                {
                    Label = "LosAngeles"
                },
                Name = "filter-store"
            });

            var mo = new MessageHandlerOptions(ExceptionHandle)
            {
                AutoComplete = true
            };
            subscriptionClient.RegisterMessageHandler(Handle, mo);
            Console.ReadLine();
        }

        private Task Handle(Message arg1, CancellationToken arg2)
        {
            NewOrderMessage newOrderMessage = JsonConvert.DeserializeObject<NewOrderMessage>(Encoding.UTF8.GetString(arg1.Body));
            _orderSqlRepository.Add(newOrderMessage);
            return Task.CompletedTask;
        }

        private Task ExceptionHandle(ExceptionReceivedEventArgs arg)
        {
            // Gravar no banco de dados o erro
            return Task.CompletedTask;
        }
    }
}
