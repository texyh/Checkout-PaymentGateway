using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Domain.Payments;
using PaymentGateway.Domain.Payments.Commands;

namespace PaymentGateway.Api.UseCases.ProcessPayment
{
    [Route("api/payments")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IProcessPaymentCommandHandler _handler;

        public PaymentController(IProcessPaymentCommandHandler handler)
        {
            _handler = handler;
        }

        [HttpPost]
        public async Task<IActionResult> ProcessPayment([FromBody]PaymentRequest request)
        {
            var command = new ProcessPaymentCommand
            {
                Amount = request.Amount,
                Currency = request.Currency,
                CardExpiryYear = request.CardExpiryYear,
                CardExpiryMonth = request.CardExpiryMonth,
                CardNumber = request.CardNumber,
                CVV = request.CVV,
                MerchantId = request.MerchantId
            };

            return Result.For(await _handler.HandleAsync(command));
        }
    }
}