using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Domain.AcquiringBank
{
    public class BankPaymentResponse
    {
        public string PaymentIdentifier { get; set; }

        public string PaymentStatus { get; set; }
    }
}
