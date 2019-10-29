using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Payment.Domain.Model;
using System;
using System.Threading.Tasks;

namespace Payment.Infrastructure.Repositories
{
    public class PaymentDataRepository : IPaymentDataRepository
    {
        private readonly PaymentDataContext _context;

        public PaymentDataRepository(IOptions<PaymentDatabaseSettings> settings)
        {
            _context = new PaymentDataContext(settings) ?? throw new ArgumentNullException(nameof(settings));
        }

        public async Task<PaymentData> Add(PaymentData paymentData)
        {
            var collection = _context.PaymentData;

            await collection.InsertOneAsync(paymentData);

            return paymentData;
        }

        public async Task<PaymentData> GetAsync(string paymentId)
        {
            var filter = Builders<PaymentData>.Filter.Eq("PaymentId", paymentId);

            return await _context.PaymentData
                                 .Find(filter)
                                 .FirstOrDefaultAsync();
        }
    }
}
