using PaymentGateway.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Domain.Payments
{
    public class Payment : Entity<string>
    {
        public string CardNumber { get; set; }

        public string CardExpiryMonth { get; set; }

        public string CardExpiryYear { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public string CVV { get; set; }

        public string EncriptionKey { get; set; }

        public string BankPaymentIdentifier { get; set; }

        public string MerchantId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
