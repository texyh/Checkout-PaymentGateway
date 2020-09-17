using Moq;
using PaymentGateway.Api.UseCases.ProcessPayment;
using PaymentGateway.Domain.Crypto;
using PaymentGateway.Domain.Payments;
using PaymentGateway.Domain.Payments.Commands;
using PaymentGateway.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PaymentGateway.UnitTests.UseCases.ProcessPayment
{
    public class PaymentProcessHandlerShould
    {

        [Fact]
        public async Task Proccess_And_Persist_Payment()
        {
            var mockPaymentRepository = new Mock<IPaymentRepository>();
            var mockCyptoService = new Mock<ICryptoService>();
            mockCyptoService.Setup(x => x.Encrypt(It.IsAny<string>(), It.IsAny<string>()))
                .Returns("__encripted__");
            var sut = new ProcessPaymentCommandHandler(mockPaymentRepository.Object, mockCyptoService.Object);
            ProcessPaymentCommand command = new ProcessPaymentCommand
            {
                Amount = 100,
                Currency = "EUR",
                CardExpiryYear = "24",
                CardExpiryMonth = "4",
                CardNumber = "5564876598743467",
                CVV = "782",
            };

            await sut.HandleAsync(command);

            mockCyptoService.Verify(x => x.Encrypt(command.CardNumber, It.IsAny<string>()), Times.Once);
            mockPaymentRepository.Verify(x => x.Save(It.Is<Payment>(y => y.CardNumber == "__encripted__")), Times.Once);
        }

    }
}
