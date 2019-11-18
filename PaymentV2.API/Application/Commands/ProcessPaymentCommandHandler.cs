using MediatR;
using Payment.Domain.AggregatesModel.PaymentAggregate;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentV2.API.Application.Commands
{
    public class ProcessPaymentCommandHandler : IRequestHandler<ProcessPaymentCommand, bool>
    {
        private readonly IPaymentRepository _paymentRepository;

        public ProcessPaymentCommandHandler(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<bool> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
        {
            // Simulação de processamento de pagamento
            await Task.Delay(10000, cancellationToken);

            var paymentToUpdate = await _paymentRepository.GetAsync(request.OrderId);

            if (paymentToUpdate == null)
                return false;

            paymentToUpdate.SetStatus();

            return await _paymentRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        }
    }
}
