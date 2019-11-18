using System;
using System.Threading.Tasks;
using Payment.Domain.Interfaces;

namespace Payment.Domain.AggregatesModel.PaymentAggregate
{
    public interface IPaymentRepository : IRepository<Payment>
    {
        Payment Add(Payment payment);

        void Update(Payment payment);

        Task<Payment> GetAsync(Guid orderId);
    }
}
