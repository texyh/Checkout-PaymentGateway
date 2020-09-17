using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.Api.UseCases.ProcessPayment
{
    public class PaymentRequest
    {
        [Required]
        [MinLength(16)]
        public string CardNumber { get; set; }

        [Required]
        public string CardExpiryMonth { get; set; }

        [Required]
        public string CardExpiryYear { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string Currency { get; set; }

        [Required]
        public string CVV { get; set; }

        [Required]
        public string MerchantId { get; set; }
    }
}
