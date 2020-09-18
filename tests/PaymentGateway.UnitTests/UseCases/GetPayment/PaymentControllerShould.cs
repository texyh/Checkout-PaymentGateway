using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PaymentGateway.Api.UseCases.GetPayment;
using PaymentGateway.Domain.Payments;
using PaymentGateway.Domain.Payments.Queries;
using Xunit;

namespace PaymentGateway.UnitTests.UseCases.GetPayment
{
    public class PaymentControllerShould
    {
        
        [Fact]
        public async Task GetPayment()
        {
            var mockqueryHandler = new Mock<IGetPaymentQueryHandler>();
            var expectedValue = new SuccessResult 
            {
                CardNumber = "***************3467",
                Amount = 100,
                PaymentDate = DateTime.UtcNow,
                PaymentStatus = PaymentStatus.Success,
                Currency = "EUR"
            };
            mockqueryHandler.Setup(x => x.HandleAsync(It.IsAny<GetPaymentQuery>()))
            .ReturnsAsync(expectedValue);
            var sut = new PaymentController(mockqueryHandler.Object);
            var paymentId = Guid.NewGuid().ToString();

            var response = await sut.GetPayment(paymentId);

            var httpResponse = response as OkObjectResult;
            httpResponse.Value.Should().Be(expectedValue);
        }

        [Fact]
        public async Task Return_Error_If_No_Payment_Is_Found() 
        {
            var mockqueryHandler = new Mock<IGetPaymentQueryHandler>();
            mockqueryHandler.Setup(x => x.HandleAsync(It.IsAny<GetPaymentQuery>()))
                            .ReturnsAsync((GetPaymentQuery query) => new ErrorResult($"There is no payment with id: {query.PaymentId}"));
            var sut = new PaymentController(mockqueryHandler.Object);
            var paymentId = Guid.NewGuid().ToString();

            var response = await sut.GetPayment(paymentId);

            var httpResponse = response as NotFoundObjectResult;
            httpResponse.Value.Should().Be($"There is no payment with id: {paymentId}");
        }
    }
}