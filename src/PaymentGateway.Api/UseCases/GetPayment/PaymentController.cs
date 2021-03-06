﻿using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Application.Payments.GetPayment;

namespace PaymentGateway.Api.UseCases.GetPayment
{
    [Route ("api/payments")]
    [ApiController]
    public class PaymentController : ControllerBase 
    {
        private readonly IMediator _mediator;

        public PaymentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet ("{id}")]
        public async Task<IActionResult> GetPayment (string id) 
        {
           return Result.For(await _mediator.Send(new GetPaymentQuery{ PaymentId = id}));
        }
    }
}