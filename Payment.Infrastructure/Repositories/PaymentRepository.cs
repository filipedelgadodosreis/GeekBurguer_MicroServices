using Microsoft.EntityFrameworkCore;
using Payment.Domain.AggregatesModel.PaymentAggregate;
using Payment.Domain.SeedWork;
using System;
using System.Threading.Tasks;

namespace Payment.Infrastructure.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly PaymentContext _context;

        public IUnitOfWork UnitOfWork
        {
            get
            {
                return _context;
            }
        }

        public PaymentRepository(PaymentContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Domain.AggregatesModel.PaymentAggregate.Payment Add(Domain.AggregatesModel.PaymentAggregate.Payment payment)
        {
            return _context.Payments.Add(payment).Entity;
        }

        public void Update(Domain.AggregatesModel.PaymentAggregate.Payment payment)
        {
            _context.Entry(payment).State = EntityState.Modified;
        }

        public async Task<Domain.AggregatesModel.PaymentAggregate.Payment> GetAsync(Guid orderId)
        {
            var payment = await _context.Payments.FindAsync(orderId);

            return payment;
        }
    }
}