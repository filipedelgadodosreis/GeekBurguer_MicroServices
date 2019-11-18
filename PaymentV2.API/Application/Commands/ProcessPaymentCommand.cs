using MediatR;
using System;
using System.Runtime.Serialization;

namespace PaymentV2.API.Application.Commands
{
    public class ProcessPaymentCommand : IRequest<bool>
    {
        [DataMember]
        public Guid OrderId { get; private set; }

        public ProcessPaymentCommand(Guid orderId)
        {
            OrderId = orderId;
        }
    }
}
