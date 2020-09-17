using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Domain.Payments.Queries;

namespace PaymentGateway.Api.UseCases.GetPayment
{
    [Route ("api/payments")]
    [ApiController]
    public class PaymentController : ControllerBase 
    {
        private readonly IGetPaymentQueryHandler _queryHandler;

        public PaymentController(IGetPaymentQueryHandler queryHandler)
        {
            _queryHandler = queryHandler;
        }

        [HttpGet ("{id}")]
        public async Task<IActionResult> GetPayment (string id) 
        {
           return Result.For(await _queryHandler.HandleAsync(new GetPaymentQuery{ PaymentId = id}));
        }
    }
}