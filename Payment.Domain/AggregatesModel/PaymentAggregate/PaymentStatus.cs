using Payment.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Payment.Domain.AggregatesModel.PaymentAggregate
{
    public class PaymentStatus : Enumeration
    {
        public static readonly PaymentStatus InProgress = new PaymentStatus(1, nameof(InProgress).ToLowerInvariant());
        public static readonly PaymentStatus Paid = new PaymentStatus(2, nameof(Paid).ToLowerInvariant());
        public static readonly PaymentStatus Canceled = new PaymentStatus(3, nameof(Canceled).ToLowerInvariant());
        public static readonly PaymentStatus Finished = new PaymentStatus(4, nameof(Finished).ToLowerInvariant());

        public PaymentStatus(int id, string name)
            : base(id, name)
        {

        }

        public static IEnumerable<PaymentStatus> List() => new[] { InProgress, Paid, Canceled, Finished };

        public static PaymentStatus FromName(string name)
        {
            var state = List().SingleOrDefault(s => string.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new ArgumentException($"Possible values for PaymentStatus: {string.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static PaymentStatus From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new ArgumentException($"Possible values for OrderStatus: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
    }
}
