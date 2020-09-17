using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Domain.Payments
{
    public interface IProcessPaymentCommandHandler
    {
        Task<ProcessPaymentResult> HandleAsync(ProcessPaymentCommand command);
    }
}
