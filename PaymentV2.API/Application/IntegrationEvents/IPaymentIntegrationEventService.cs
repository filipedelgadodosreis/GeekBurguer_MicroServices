using EventBus.Events;

namespace PaymentV2.API.Application.IntegrationEvents
{
    public interface IPaymentIntegrationEventService
    {
        void AddEvent(IntegrationEvent evt);
    }
}
