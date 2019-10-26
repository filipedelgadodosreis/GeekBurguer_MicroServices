using MediatR;
using System;
using System.Runtime.Serialization;

namespace Payment.API.Application.Commands
{
    public class SetPaidOrderStatusCommand : IRequest<bool>
    {
        [DataMember]
        public Guid OrderId { get; private set; }

        public SetPaidOrderStatusCommand(Guid orderId)
        {
            OrderId = orderId;
        }
    }
}
