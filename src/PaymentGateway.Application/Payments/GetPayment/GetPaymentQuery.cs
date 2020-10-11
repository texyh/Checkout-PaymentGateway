using PaymentGateway.Application.Abstractions.Queries;

namespace PaymentGateway.Application.Payments.GetPayment
{
    public class GetPaymentQuery : IQuery<GetPaymentResult>
    {
        public string PaymentId { get; set; }
    }
}