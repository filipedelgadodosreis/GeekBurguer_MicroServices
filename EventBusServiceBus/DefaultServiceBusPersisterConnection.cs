using System;
using Microsoft.Azure.ServiceBus;


namespace EventBusServiceBus
{
    public class DefaultServiceBusPersisterConnection : IServiceBusPersisterConnection
    {
        bool _disposed;

        private ITopicClient _topicClient;
        private readonly ServiceBusConnectionStringBuilder _serviceBusConnectionStringBuilder;

        public DefaultServiceBusPersisterConnection(ServiceBusConnectionStringBuilder serviceBusConnectionStringBuilder)
        {
            _serviceBusConnectionStringBuilder = serviceBusConnectionStringBuilder ?? throw new ArgumentNullException(nameof(serviceBusConnectionStringBuilder));
            _topicClient = new TopicClient(_serviceBusConnectionStringBuilder, RetryPolicy.Default);
        }


        public ServiceBusConnectionStringBuilder ServiceBusConnectionStringBuilder => throw new NotImplementedException();

        public ITopicClient CreateModel()
        {
            if (_topicClient.IsClosedOrClosing)
            {
                _topicClient = new TopicClient(_serviceBusConnectionStringBuilder, RetryPolicy.Default);
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
