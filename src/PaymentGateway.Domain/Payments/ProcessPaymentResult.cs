using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Domain.Payments
{
    public abstract class ProcessPaymentResult
    {
    }

    public class SuccessResult : ProcessPaymentResult
    {
        public string PaymentId { get; }

        public SuccessResult(string paymentId)
        {
            PaymentId = paymentId;
        }

    }

    public class ErrorResult : ProcessPaymentResult
    {
        public string Message { get; }

        public ErrorResult(string message)
        {
            Message = message;
        }
    }
}
