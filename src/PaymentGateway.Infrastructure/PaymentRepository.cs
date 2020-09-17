using PaymentGateway.Domain.Payments;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Infrastructure
{
    public class PaymentRepository : IPaymentRepository
    {
        public DbContext _dbContext { get; }

        public PaymentRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task Save(Payment payment)
        {
            _dbContext.Payments.Add(payment);

            return Task.CompletedTask;
        }

        public Task<Payment> FindBy(string id)
        {
            var payment = _dbContext.Payments.Find(x => x.Id == id);

            return Task.FromResult(payment);
        }
    }
}
