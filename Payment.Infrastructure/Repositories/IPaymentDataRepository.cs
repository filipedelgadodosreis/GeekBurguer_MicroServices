using System;
using Payment.Domain.Model;
using System.Threading.Tasks;

namespace Payment.Infrastructure.Repositories
{
    public interface IPaymentDataRepository
    {
        Task<PaymentData> Add(PaymentData paymentData);

        Task<PaymentData> GetAsync(string paymentId);
    }
}
