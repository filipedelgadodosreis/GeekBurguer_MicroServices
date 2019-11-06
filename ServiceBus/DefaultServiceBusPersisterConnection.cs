using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using System;

namespace ServiceBus
{
    public class DefaultServiceBusPersisterConnection : IServiceBusPersisterConnection
    {
        bool _disposed;

        private ITopicClient _topicClient;
        private readonly ILogger<DefaultServiceBusPersisterConnection> _logger;

        private readonly string _topicName;

        public DefaultServiceBusPersisterConnection(string serviceBusConnectionString, ILogger<DefaultServiceBusPersisterConnection> logger,
            string topicName)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _topicName = topicName ?? throw new ArgumentNullException(nameof(topicName));
            ServiceBusConnectionStringBuilder = serviceBusConnectionString ?? throw new ArgumentNullException(nameof(serviceBusConnectionString));

            _topicClient = new TopicClient(ServiceBusConnectionStringBuilder, _topicName, RetryPolicy.Default);
        }


        public string ServiceBusConnectionStringBuilder { get; }

        public ITopicClient CreateModel()
        {
            if (_topicClient.IsClosedOrClosing)
            {
                _topicClient = new TopicClient(ServiceBusConnectionStringBuilder, _topicName, RetryPolicy.Default);
            }

            return _topicClient;
        }

        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;
        }

    }
}
