using FluentValidation;
using PaymentGateway.Domain.Payments.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Application.Payments.ProcessPayment
{
    public class ProcessPaymentCommandValidator : AbstractValidator<ProcessPaymentCommand>
    {
        public ProcessPaymentCommandValidator()
        {
            // TODO
        }
    }
}
