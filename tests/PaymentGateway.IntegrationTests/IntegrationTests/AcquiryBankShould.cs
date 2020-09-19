using PaymentGateway.Domain.AcquiringBank;
using PaymentGateway.Domain.Payments;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using Xunit;
using PaymentGateway.Domain.Helpers;
using PaymentGateway.Framework;
using Microsoft.Extensions.Options;
using Moq;
using System.Linq;
using System.Net.Http;
using FluentAssertions;
using PaymentGateway.Infrastructure;

namespace PaymentGateway.IntegrationTests.IntegrationTests
{
    public class AcquiryBankShould
    {
        private readonly WireMockServer _server;

        private readonly Mock<IOptionsMonitor<AcquiringBankSettings>> _mockAcquiryBankOptions;


        public AcquiryBankShould()
        {
            _server = WireMockServer.Start();
            _mockAcquiryBankOptions = new Mock<IOptionsMonitor<AcquiringBankSettings>>();
        }


        [Fact]
        public async Task Return_Success_Response_When_Request_Is_Successful()
        {
            BankPaymentResponse bankPaymentResponse = (new BankPaymentResponse { PaymentIdentifier = Guid.NewGuid().ToString(), PaymentStatus = PaymentStatus.Success });
            _server
                .Given(Request.Create()
                .WithPath($"/api/payment").UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBody(bankPaymentResponse.ToJson()));
            _mockAcquiryBankOptions.SetupGet(x => x.CurrentValue)
                .Returns(new AcquiringBankSettings { ApiUrl = _server.Urls.First()});
            var sut = new AcquiringBankClient(new HttpClient(), _mockAcquiryBankOptions.Object);


            var response = await sut.ProcessPayment(new BankPaymentRequest
            {
                Amount = 100,
                Currency = "EUR",
                CardExpiryYear = "24",
                CardExpiryMonth = "4",
                CardNumber = "5564876598743467",
                CVV = "782",
                MerchantId = Guid.NewGuid().ToString()
            });

            response.Should().BeEquivalentTo(bankPaymentResponse);
        }

        [Fact]
        public async Task Return_Failed_Response_When_Request_Is_Not_SuccessFul()
        {
            _server
                .Given(Request.Create()
                .WithPath($"/api/payment").UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(500));
            _mockAcquiryBankOptions.SetupGet(x => x.CurrentValue)
                .Returns(new AcquiringBankSettings { ApiUrl = _server.Urls.First() });
            var sut = new AcquiringBankClient(new HttpClient(), _mockAcquiryBankOptions.Object);


            var response = await sut.ProcessPayment(new BankPaymentRequest
            {
                Amount = 100,
                Currency = "EUR",
            });

            response.PaymentStatus.Should().Be(PaymentStatus.Failed);
            response.PaymentIdentifier.Should().BeNull();
        }
    }
}
