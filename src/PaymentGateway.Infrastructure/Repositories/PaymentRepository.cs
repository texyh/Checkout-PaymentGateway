using Microsoft.EntityFrameworkCore;
using PaymentGateway.Domain.Payments;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Infrastructure
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly PaymentGatewayDbContext _dbContext;

        public PaymentRepository(PaymentGatewayDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task Save(Payment payment)
        {
            _dbContext.Payments.Add(payment);

            return _dbContext.SaveChangesAsync();
        }

        public async Task<Payment> FindBy(string id)
        {
            return await _dbContext.Payments.SingleAsync(x => x.Id == id);
        }
    }
}
