using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Domain.Payments
{
    public interface IPaymentRepository
    {
        Task Save(Payment payment);

        Task<Payment> FindBy(string Id);
    }
}
