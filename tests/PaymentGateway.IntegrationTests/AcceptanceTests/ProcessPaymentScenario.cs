using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using PaymentGateway.Api;
using PaymentGateway.Api.UseCases.ProcessPayment;
using PaymentGateway.Domain.Payments;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xbehave;
using Environment = PaymentGateway.Api.Environment;
using Serilog;
using Xunit;
using System.Threading;

namespace PaymentGateway.IntegrationTests.AcceptanceTests
{
    [Collection("Acceptance-Tests")]
    public class ProcessPaymentScenario : IClassFixture<TestServerFixture>
    {
        private TestServerFixture _fixture;

        public ProcessPaymentScenario(TestServerFixture fixture)
        {
            fixture.CreateTestEnvironment(services => 
            {
                services.AddProcessPaymentUseCase();
            });
            _fixture = fixture;
        }


        [Scenario]
        public void Process_Payment(PaymentRequest paymentRequest, PaymentResponse paymentResponse)
        {
            "Given i have a payment request"
                .x(() => paymentRequest =  GivenPaymentRequest());

            "When i make a payment request"
                .x(async () =>
                {
                     paymentResponse = await MakePaymentRequest(paymentRequest);
                     paymentResponse.Should().NotBeNull();
                     paymentResponse.PaymentId.Should().NotBeNull();
                });

            "And the saved card details should be encripted"
                .x(async () => await AssertPaymentIsPersistAndCardDetailsAreEncripted(paymentResponse, paymentRequest));
        }

        private async Task AssertPaymentIsPersistAndCardDetailsAreEncripted(PaymentResponse paymentResponse, PaymentRequest paymentRequest)
        {
            var paymentRepository = _fixture.GetService<IPaymentRepository>();
            var payment = await paymentRepository.Load(Guid.Parse(paymentResponse.PaymentId));

            payment.CardNumber.Should().NotBeEquivalentTo(paymentRequest.CardNumber);
            payment.CardExpiryMonth.Should().NotBeEquivalentTo(paymentRequest.CardExpiryMonth);
            payment.CardExpiryYear.Should().NotBeEquivalentTo(paymentRequest.CardExpiryYear);
            payment.CVV.Should().NotBeEquivalentTo(paymentRequest.CVV);
            payment.PaymentStatus.Should().Be(PaymentStatus.Success);
        }

        private async Task<PaymentResponse> MakePaymentRequest(PaymentRequest paymentRequest)
        {
            var cancellation = new CancellationTokenSource();
            cancellation.CancelAfter(TimeSpan.FromSeconds(30));
            return await _fixture.PostAsync<PaymentResponse, PaymentRequest>("api/payments", paymentRequest, cancellation.Token);
        }

        private PaymentRequest GivenPaymentRequest()
        {
            return new PaymentRequest
            {
                Amount = 100,
                Currency = "EUR",
                CardExpiryYear = "24",
                CardExpiryMonth = "4",
                CardNumber = "5564876598743467",
                CVV = "782",
                MerchantId = Guid.NewGuid().ToString()
            };
        }

    }
}
