namespace PaymentGateway.Domain.Payments.Queries
{
    public abstract class GetPaymentResult
    {
        
    }

    public class SuccessResult : GetPaymentResult
    {
        public string CardNumber {get; set;}
    }

    public class ErrorResult : GetPaymentResult
    {
        public string Message { get; }

        public ErrorResult(string message)
        {
            Message = message;
        }
    }
}