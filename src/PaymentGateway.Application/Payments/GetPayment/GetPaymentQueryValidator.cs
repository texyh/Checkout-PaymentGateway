using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Application.Payments.GetPayment
{
    public class GetPaymentQueryValidator : AbstractValidator<GetPaymentQuery>
    {
        public GetPaymentQueryValidator()
        {
            RuleFor(x => x.PaymentId).NotEmpty().NotNull();
        }
    }
}
