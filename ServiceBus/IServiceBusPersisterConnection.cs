using Microsoft.Azure.ServiceBus;
using System;

namespace ServiceBus
{
    public interface IServiceBusPersisterConnection : IDisposable
    {
        string ServiceBusConnectionStringBuilder { get; }

        ITopicClient CreateModel();
    }
}
