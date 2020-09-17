using PaymentGateway.Domain.Payments;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Infrastructure
{
    public class DbContext
    {
        public DbContext()
        {
            Payments = new List<Payment>();
        }

        public List<Payment> Payments;
    }
}
