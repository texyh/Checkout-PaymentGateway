using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using PaymentGateway.Api.UseCases.GetPayment;
using PaymentGateway.Domain.Crypto;
using PaymentGateway.Domain.Payments;
using PaymentGateway.Domain.Payments.Queries;
using PaymentGateway.Domain.Helpers;
using Xunit;

namespace PaymentGateway.UnitTests.UseCases.GetPayment
{
    public class GetPaymentQueryHandlerShould
    {
        private const string cardNumber = "5564876598743467";

        [Fact]
        public async Task Get_Payment_Details() 
        {
            var paymentId = Guid.NewGuid().ToString();
            var payment = GivenPayment(paymentId);
            var mockPaymentRepository = new Mock<IPaymentRepository>();
            mockPaymentRepository.Setup(x => x.FindBy(It.IsAny<string>()))
                                 .ReturnsAsync(payment);
            var mockCyptoService = new Mock<ICryptoService>();
            mockCyptoService.Setup(x => x.Decrypt(payment.CardNumber, It.IsAny<string>()))
                            .Returns(cardNumber);
            var sut = new GetPaymentQueryHandler(mockPaymentRepository.Object, mockCyptoService.Object);

            var result = await sut.HandleAsync(new GetPaymentQuery{PaymentId = paymentId}) as SuccessResult;

            result.Amount.Should().Be(payment.Amount);
            result.PaymentDate.Should().Be(payment.CreatedDate);
            result.Currency.Should().Be(payment.Currency);
            result.CardNumber.Should().Be(cardNumber.Mask());
        }

        [Fact]
        public async Task Return_Error_Result_If_Payment_NotFound() 
        {
            var paymentId = Guid.NewGuid().ToString();
            var mockPaymentRepository = new Mock<IPaymentRepository>();
            mockPaymentRepository.Setup(x => x.FindBy(It.IsAny<string>()))
                .ReturnsAsync(null as Payment);
            var mockCyptoService = new Mock<ICryptoService>();
            var sut = new GetPaymentQueryHandler(mockPaymentRepository.Object, mockCyptoService.Object);

            var result = await sut.HandleAsync(new GetPaymentQuery{PaymentId = paymentId}) as ErrorResult;

            result.Message.Should().Be($"There is no payment with id: {paymentId}");
        }

        private Payment GivenPayment(string id) 
        {
            return new Payment
            {
                Id = id,
                EncriptionKey = "20ba9d3d123141c8b0ae4df0a3383f7e",
                CardNumber = "7bedd30c790f45c6410b7389a58d2cbe.a159e99d6f97d3fbc60fe88161d54edac66a5c323521b7b2281465824bdcbae0",
                CardExpiryMonth = "c3bae8e66e8cf282b77ce798874e5b22.9cc552bdb6f06ebd08c7a2705fa40d28",
                CardExpiryYear = "1dbf11b97e97ea71d51bc8cf0ffee21f.2088cf0b4b3522a5230beebeaea0132a",
                CVV = "e6cbdaf95f6f694ba2b83d24b7b9403a.b07288ff12c714ccfd5ca281e84da132",
                BankPaymentIdentifier = "d4920d4e-c6e0-4b6e-a259-cab69db9f1c5",
                Amount = 100,
                Currency = "EUR",
                PaymentStatus = PaymentStatus.Success,
                MerchantId = Guid.NewGuid().ToString(),
                CreatedDate = DateTime.UtcNow
            };
        }
    }
}