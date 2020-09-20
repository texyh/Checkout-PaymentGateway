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

namespace PaymentGateway.IntegrationTests.AcceptanceTests
{
    public class ProcessPaymentScenario
    {
        private TestServer _env;

        public ProcessPaymentScenario()
        {
            _env = CreateTestEnvironment();
        }


        [Scenario]
        public void Process_Payment(PaymentRequest paymentRequest, HttpResponseMessage httpResponse, PaymentResponse paymentResponse)
        {
            "Given i have a payment request"
                .x(() => paymentRequest =  GivenPaymentRequest());

            "When i make a payment request"
                .x(async () =>
                {
                    httpResponse = await MakePaymentRequest(paymentRequest);
                });

            "Then a 201 created response is returned"
                .x(() => httpResponse.StatusCode.Should().Be(HttpStatusCode.Created));

            "Then the payment identifier should be returned"
                .x(async() =>
                {
                    var stringResult = await httpResponse.Content.ReadAsStringAsync();
                    paymentResponse = JsonSerializer.Deserialize<PaymentResponse>(stringResult, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    paymentResponse.Should().NotBeNull();
                    paymentResponse.PaymentId.Should().NotBeNull();
                });

            "And the saved card details should be encripted"
                .x(async () => await AssertPaymentIsPersistAndCardDetailsAreEncripted(paymentResponse, paymentRequest));
        }

        private async Task AssertPaymentIsPersistAndCardDetailsAreEncripted(PaymentResponse paymentResponse, PaymentRequest paymentRequest)
        {
            var paymentRepository = _env.Services.GetService<IPaymentRepository>();
            var payment = await paymentRepository.FindBy(paymentResponse.PaymentId);

            payment.CardNumber.Should().NotBeEquivalentTo(paymentRequest.CardNumber);
            payment.CardExpiryMonth.Should().NotBeEquivalentTo(paymentRequest.CardExpiryMonth);
            payment.CardExpiryYear.Should().NotBeEquivalentTo(paymentRequest.CardExpiryYear);
            payment.CVV.Should().NotBeEquivalentTo(paymentRequest.CVV);
            payment.PaymentStatus.Should().Be(PaymentStatus.Success);
        }

        private async Task<HttpResponseMessage> MakePaymentRequest(PaymentRequest paymentRequest)
        {
            var stringContent = new StringContent(JsonSerializer.Serialize(paymentRequest), Encoding.UTF8, "application/json");

            return await _env.CreateClient().PostAsync("api/payments", stringContent);
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

        private TestServer CreateTestEnvironment()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            Environment.SetToDevelopment();

            var settings = new List<KeyValuePair<string, string>>();
            settings.Add(new KeyValuePair<string, string>("BANK_API_URL", "http://localhost:5002"));
            settings.AddRange(
                new List<KeyValuePair<string, string>> 
                {
                    new KeyValuePair<string, string>("POSTGRES_USERNAME", "root"),
                    new KeyValuePair<string, string>("POSTGRES_PASSWORD", "Pass@word1"),
                    new KeyValuePair<string, string>("POSTGRES_HOST", "192.168.99.100"),
                    new KeyValuePair<string, string>("POSTGRES_DB_NAME", "paymanetGatewayDb"),
                    new KeyValuePair<string, string>("POSTGRES_PORT", "5432"),
                }
            );
            var configuration = new ConfigurationBuilder().AddInMemoryCollection(settings).Build();

            var server = new TestServer(new WebHostBuilder()
                .UseConfiguration(configuration)
                .UseStartup<Startup>()
                .ConfigureServices(services =>
                {
                    services.AddProcessPaymentUseCase();
                    services.AddPostgres(configuration);
                }).UseSerilog(Log.Logger));

            return server;
        }
    
    }
}
