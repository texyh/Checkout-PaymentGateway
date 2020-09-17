using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Domain.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.Api.UseCases.ProcessPayment
{
    public class Result
    {
        public static IActionResult For(ProcessPaymentResult output)
        {
            return output switch
            {
                SuccessResult result => new CreatedResult($"api/payments/{result.PaymentId}", new PaymentResponse { PaymentId =  result.PaymentId }),
                _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
            };
        }
    }
}
