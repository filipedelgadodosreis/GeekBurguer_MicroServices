using System;
using Autofac;
using EventBus;
using System.Text;
using EventBus.Events;
using EventBus.Abstractions;
using Microsoft.Azure.ServiceBus;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace EventBusServiceBus
{
    public class EventBusServiceBus : IEventBus
    {
        private readonly ILifetimeScope _autofac;
        private readonly SubscriptionClient _subscriptionClient;
        private readonly IEventBusSubscriptionsManager _subsManager;
        private readonly IServiceBusPersisterConnection _serviceBusPersisterConnection;

        private readonly string AUTOFAC_SCOPE_NAME = "geekBurger_event_bus";
        private const string INTEGRATION_EVENT_SUFIX = "IntegrationEvent";

        public EventBusServiceBus(IServiceBusPersisterConnection serviceBusPersisterConnection, string subscriptionClientName, IEventBusSubscriptionsManager subsManager,
            ILifetimeScope autofac)
        {
            _autofac = autofac;
            _serviceBusPersisterConnection = serviceBusPersisterConnection;
            _subsManager = subsManager ?? new InMemoryEventBusSubscriptionsManager();
            _subscriptionClient = new SubscriptionClient(serviceBusPersisterConnection.ServiceBusConnectionStringBuilder, subscriptionClientName);

            RemoveDefaultRule();
            RegisterSubscriptionClientMessageHandler();
        }

        public void Publish(IntegrationEvent @event)
        {
            throw new NotImplementedException();
        }

        public void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            throw new NotImplementedException();
        }

        public void SubscribeDynamic<TH>(string eventName) where TH : IDynamicIntegrationEventHandler
        {
            throw new NotImplementedException();
        }

        public void Unsubscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            throw new NotImplementedException();
        }

        public void UnsubscribeDynamic<TH>(string eventName) where TH : IDynamicIntegrationEventHandler
        {
            throw new NotImplementedException();
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
                // _logger.LogWarning("A mensagem da entidade {DefaultRuleName} não pode ser encontrada.", RuleDescription.DefaultRuleName);
            }
        }

        private void RegisterSubscriptionClientMessageHandler()
        {
            _subscriptionClient.RegisterMessageHandler(
                async (message, token) =>
                {
                    var eventName = $"{message.Label}{INTEGRATION_EVENT_SUFIX}";
                    var messageData = Encoding.UTF8.GetString(message.Body);

                    if (await ProcessEvent(eventName, messageData))
                    {
                        await _subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
                    }
                },
                new MessageHandlerOptions(ExceptionReceivedHandler) { MaxConcurrentCalls = 10, AutoComplete = false });
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            var ex = exceptionReceivedEventArgs.Exception;
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;

            //_logger.LogError(ex, "ERROR no tratamento da mensagem: {ExceptionMessage} - Context: {@ExceptionContext}", ex.Message, context);

            return Task.CompletedTask;
        }

        private async Task<bool> ProcessEvent(string eventName, string message)
        {
            var processed = false;
            if (_subsManager.HasSubscriptionsForEvent(eventName))
            {
                using (var scope = _autofac.BeginLifetimeScope(AUTOFAC_SCOPE_NAME))
                {
                    var subscriptions = _subsManager.GetHandlersForEvent(eventName);
                    foreach (var subscription in subscriptions)
                    {
                        if (subscription.IsDynamic)
                        {
                            var handler = scope.ResolveOptional(subscription.HandlerType) as IDynamicIntegrationEventHandler;
                            if (handler == null) continue;
                            dynamic eventData = JObject.Parse(message);
                            await handler.Handle(eventData);
                        }
                        else
                        {
                            var handler = scope.ResolveOptional(subscription.HandlerType);
                            if (handler == null) continue;
                            var eventType = _subsManager.GetEventTypeByName(eventName);
                            var integrationEvent = JsonConvert.DeserializeObject(message, eventType);
                            var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
                            await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });
                        }
                    }
                }
                processed = true;
            }
            return processed;
        }
    }
}
