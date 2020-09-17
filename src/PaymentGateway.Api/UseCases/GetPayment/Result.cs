using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Domain.Payments.Commands;
using PaymentGateway.Domain.Payments.Queries;
using ErrorResult = PaymentGateway.Domain.Payments.Queries.ErrorResult;
using SuccessResult = PaymentGateway.Domain.Payments.Queries.SuccessResult;

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