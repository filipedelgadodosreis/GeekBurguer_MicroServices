namespace ServiceBusMessaging
{
    public interface IServiceBusTopicSubscription
    {
        void RegisterOnMessageHandlerAndReceiveMessages();
    }
}
