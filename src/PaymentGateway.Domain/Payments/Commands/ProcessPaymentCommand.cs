namespace PaymentGateway.Domain.Payments.Commands
{
    public class ProcessPaymentCommand
    {
        public string CardNumber { get; set; }

        public string CardExpiryMonth { get; set; }

        public string CardExpiryYear { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public string CVV { get; set; }

        public string MerchantId { get; set; }
    }
}
