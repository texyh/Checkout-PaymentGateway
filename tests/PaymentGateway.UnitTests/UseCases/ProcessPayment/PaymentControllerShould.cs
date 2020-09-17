using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PaymentGateway.Api.UseCases.ProcessPayment;
using PaymentGateway.Domain.Payments;
using PaymentGateway.Domain.Payments.Commands;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PaymentGateway.UnitTests.UseCases.ProcessPayment
{
    public class PaymentControllerShould
    {


        [Fact]
        public async Task ProcessPayment_And_Return_PaymentId()
        {
            var handler = new Mock<IProcessPaymentCommandHandler>();
            var paymentId = Guid.NewGuid().ToString();
            handler.Setup(x => x.HandleAsync(It.IsAny<ProcessPaymentCommand>()))
                .ReturnsAsync(new SuccessResult(paymentId));
            var expectedResponseValue = new PaymentResponse
            {
               PaymentId = paymentId
            };
            var sut = new PaymentController(handler.Object); //subject under test.

            var response = await sut.ProcessPayment(new PaymentRequest
            {
                Amount = 100,
                Currency = "EUR",
                CardExpiryYear = "24",
                CardExpiryMonth = "4",
                CardNumber = "5564876598743467",
                CVV = "782",
            });

            var httpResponse = response as CreatedResult;
            httpResponse.Location.Should().Be($"api/payments/{paymentId}");
            httpResponse.StatusCode.Should().Be((int)HttpStatusCode.Created);
            httpResponse.Value.Should().Be(expectedResponseValue);
        }
    }
}
