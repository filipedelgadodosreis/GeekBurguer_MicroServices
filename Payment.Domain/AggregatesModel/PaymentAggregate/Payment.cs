using Payment.Domain.Events;
using Payment.Domain.Interfaces;
using Payment.Domain.SeedWork;
using System;

namespace Payment.Domain.AggregatesModel.PaymentAggregate
{
    public class Payment
        : Entity, IAggregateRoot
    {
        private readonly int _paymentId;
        private readonly Guid _orderId;
        private readonly Guid _storeId;
        private readonly Guid _requesterId;

        public string _state;
        private string _description;


        private readonly string _payType;
        private readonly string _cardNumber;
        private readonly string _securityCode;
        private readonly string _cardOwnerName;
        private readonly DateTime _expirationDate;

        public Payment(int paymentId , Guid orderId, Guid storeId, Guid requesterId, string state, string payType, string cardNumber, string securityCode, string cardOwnerName,
           DateTime expirationDate)
        {
            _paymentId = paymentId;
            _orderId = orderId;
            _storeId = storeId;
            _requesterId = requesterId;

            _state = !string.IsNullOrWhiteSpace(state) ? state : throw new ArgumentException(nameof(state));
            _payType = !string.IsNullOrWhiteSpace(payType) ? payType : throw new ArgumentException(nameof(payType));
            _cardNumber = !string.IsNullOrWhiteSpace(cardNumber) ? cardNumber : throw new ArgumentException(nameof(cardNumber));
            _securityCode = !string.IsNullOrWhiteSpace(securityCode) ? securityCode : throw new ArgumentException(nameof(securityCode));
            _cardOwnerName = !string.IsNullOrWhiteSpace(cardOwnerName) ? cardOwnerName : throw new ArgumentException(nameof(cardOwnerName));

            _expirationDate = expirationDate;
        }

        public void SetStatus()
        {
            if (_state == PaymentStatus.InProgress.Name)
            {
                // Notifica produtos que o pagamento foi realizado com sucesso!!!
                AddDomainEvent(new OrderStatusChangedToPaidDomainEvent(_orderId));

                _state = PaymentStatus.Paid.Name;
                _description = "O pagamento foi realizado de forma simulada \"Conta corrente que termina em XX35071\"";
            }
        }
    }
}
