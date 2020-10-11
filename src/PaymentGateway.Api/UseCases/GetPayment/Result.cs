using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Application.Payments.GetPayment;
using ErrorResult = PaymentGateway.Application.Payments.GetPayment.ErrorResult;
using SuccessResult = PaymentGateway.Application.Payments.GetPayment.SuccessResult;

namespace PaymentGateway.Api.UseCases.GetPayment
{
    public static class Result
    {
        public static IActionResult For(GetPaymentResult result) 
        {
            return result switch
            {
                SuccessResult r => new OkObjectResult(r),
                ErrorResult e => new NotFoundObjectResult(e.Message),
                _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
            };
        }
    }
}