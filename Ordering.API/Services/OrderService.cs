using GeekBurger.UI.Contract;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ServiceBus.Fluent;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Ordering.API.Sql.Repositories;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.API.Services
{
    public class OrderService : IOrderService
    {
        private readonly IConfiguration _configuration;
        private readonly OrderSqlRepository _orderSqlRepository;
        private readonly IServiceBusNamespace _namespace;

        public OrderService(IConfiguration configuration, OrderSqlRepository orderSqlRepository)
        {
            _configuration = configuration;
            _orderSqlRepository = orderSqlRepository;
            _namespace = GetServiceBusNamespace();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            EnsureTopicIsCreated();
            EnsureSubscriptionIsCreated();
            GetOrder();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private IServiceBusNamespace GetServiceBusNamespace()
        {
            var credentials = SdkContext.AzureCredentialsFactory.FromServicePrincipal(
                _configuration["serviceBus:clientId"],
                _configuration["serviceBus:clientSecret"],
                _configuration["serviceBus:tenantId"],
                AzureEnvironment.AzureGlobalCloud);

            var serviceBusManager = ServiceBusManager.Authenticate(credentials, _configuration["serviceBus:subscriptionId"]);
            return serviceBusManager.Namespaces.GetByResourceGroup(_configuration["serviceBus:resourceGroup"], _configuration["serviceBus:namespaceName"]);
        }

        private async void GetOrder()
        {
            string connectionString = _configuration["serviceBus:connectionString"];
            var subscriptionClient = new Microsoft.Azure.ServiceBus.SubscriptionClient(connectionString, "newOrder", "mySubscrition");
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
        }

        private void EnsureTopicIsCreated()
        {
            if (!_namespace.Topics.List().Any(topic => topic.Name.Equals("newOrder", StringComparison.InvariantCultureIgnoreCase)))
            {
                _namespace.Topics.Define("newOrder").WithSizeInMB(1024).Create();
            }
        }

        private void EnsureSubscriptionIsCreated()
        {
            if (!_namespace.Topics.List().Any(topic => topic.Subscriptions.List().Any(s=>s.Name.Equals("mySubscrition", StringComparison.InvariantCultureIgnoreCase))))
            {
                _namespace.Topics.GetByName("newOrder").Subscriptions.Define("mySubscrition").Create();
            }
        }

        private Task Handle(Message arg1, CancellationToken arg2)
        {
            NewOrderMessage newOrderMessage = JsonConvert.DeserializeObject<NewOrderMessage>(Encoding.UTF8.GetString(arg1.Body));
            _orderSqlRepository.Add(newOrderMessage);
            return Task.CompletedTask;
        }

        private Task ExceptionHandle(ExceptionReceivedEventArgs arg)
        {
            // Logar o erro 
            return Task.CompletedTask;
        }
    }
}
