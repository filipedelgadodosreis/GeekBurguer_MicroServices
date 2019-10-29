using MediatR;
using Payment.Infrastructure.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Payment.API.Application.Commands
{
    public class SetPaidOrderStatusCommandHandler : IRequestHandler<SetPaidOrderStatusCommand, bool>
    {
        private readonly IPaymentDataRepository _paymentDataRepository;

        public SetPaidOrderStatusCommandHandler(IPaymentDataRepository paymentDataRepository)
        {
            _paymentDataRepository = paymentDataRepository ?? throw new ArgumentNullException(nameof(paymentDataRepository));
        }

        public async Task<bool> Handle(SetPaidOrderStatusCommand command, CancellationToken cancellationToken)
        {
            // Simulação do tempo de processamento do pagamento
            await Task.Delay(10000, cancellationToken);

            //await _paymentDataRepository.UpdateLocationAsync(userMarketingData);

            _paymentDataRepository.Add(command.)
        }
    }
}
