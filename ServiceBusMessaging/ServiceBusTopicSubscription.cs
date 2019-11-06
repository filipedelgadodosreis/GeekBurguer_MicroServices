using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;

namespace ServiceBusMessaging
{
    public class ServiceBusTopicSubscription : IServiceBusTopicSubscription
    {
        private readonly ISubscriptionClient _subscriptionClient;
        private readonly ILogger<ServiceBusTopicSubscription> _logger;

        public ServiceBusTopicSubscription(string serviceBusConnectionString, string topicName, string subscriptionName, ILogger<ServiceBusTopicSubscription> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _subscriptionClient = new SubscriptionClient(serviceBusConnectionString, topicName, subscriptionName);

            RemoveDefaultRule();
            RegisterOnMessageHandlerAndReceiveMessages();
        }

        public void RegisterOnMessageHandlerAndReceiveMessages()
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 3,
                AutoComplete = false
            };

            _subscriptionClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }

        private async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            // Process the message.
            _logger.LogInformation($"Received message: SequenceNumber:{message.SystemProperties.SequenceNumber} Body:{Encoding.UTF8.GetString(message.Body)}");

            await _subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            var ex = exceptionReceivedEventArgs.Exception;
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;

            _logger.LogError(ex, "ERROR no tratamento da mensagem: {ExceptionMessage} - Context: {@ExceptionContext}", ex.Message, context);

            return Task.CompletedTask;
        }

        private void RemoveDefaultRule()
        {
            try
            {
                _subscriptionClient
                 .RemoveRuleAsync(RuleDescription.DefaultRuleName)
                 .GetAwaiter()
                 .GetResult();
            }
            catch (MessagingEntityNotFoundException)
            {
                 _logger.LogError("A mensagem da entidade {DefaultRuleName} não pode ser encontrada.", RuleDescription.DefaultRuleName);
            }
        }
    }
}
