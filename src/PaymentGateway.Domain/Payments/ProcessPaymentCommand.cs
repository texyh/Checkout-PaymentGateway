using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Domain.Payments
{
    public class ProcessPaymentCommand
    {
        public string CardNumber { get; set; }

        public string CardExpiryMonth { get; set; }

        public string CardExpiryYear { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public string CVV { get; set; }
    }
}
