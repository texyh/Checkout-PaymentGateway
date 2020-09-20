using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using PaymentGateway.Api;
using PaymentGateway.Api.UseCases.GetPayment;
using PaymentGateway.Domain.Payments;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xbehave;
using Microsoft.Extensions.DependencyInjection;
using FluentAssertions;
using System.Net;
using System.Text.Json;
using PaymentGateway.Domain.Payments.Queries;
using Serilog;
using Xunit;

namespace PaymentGateway.IntegrationTests.AcceptanceTests
{
    [Collection("Acceptance-Tests")]
    public class GetPaymentScenario
    {
        private TestServer _env;

        public GetPaymentScenario()
        {
            _env = CreateTestEnvironment();
        }

        [Scenario]
        public void Get_Payment(SuccessResult payment, Payment existingPayment)
        {
            "Given an existing payment"
                .x(async () => existingPayment = await CreatePayment());

            "When it is retrieved"
                .x(async () =>
                {
                    var response = await GetPayment(existingPayment.Id);
                    response.StatusCode.Should().Be(HttpStatusCode.OK);

                    var stringResult = await response.Content.ReadAsStringAsync();
                    payment = JsonSerializer.Deserialize<SuccessResult>(stringResult, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                });

            "Then it should have payment details"
                .x(() =>  {
                    payment.Currency.Should().Be(existingPayment.Currency);
                    payment.PaymentStatus.Should().Be(existingPayment.PaymentStatus);
                    payment.Amount.Should().Be(existingPayment.Amount);
                    payment.PaymentDate.Date.Should().Be(existingPayment.CreatedDate.Date);
                  }); 
        }

        private async Task<HttpResponseMessage> GetPayment(string paymentId)
        {
            return await _env.CreateClient().GetAsync($"api/payments/{paymentId}");
        }

        private async Task<Payment> CreatePayment()
        {
            var payment = new Payment
            {
                Id = Guid.NewGuid().ToString(),
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

            var paymentRepository = _env.Services.GetService<IPaymentRepository>();
            await paymentRepository.Save(payment);

            return payment;
        }

        private TestServer CreateTestEnvironment()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            var settings = new List<KeyValuePair<string, string>>();
            settings.AddRange(
                new List<KeyValuePair<string, string>> 
                {
                    new KeyValuePair<string, string>("POSTGRES_USERNAME", "root"),
                    new KeyValuePair<string, string>("POSTGRES_PASSWORD", "Pass@word1"),
                    new KeyValuePair<string, string>("POSTGRES_HOST", "localhost"),
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
                    services.AddGetPaymentUseCase();
                    services.AddPostgres(configuration);
                }).UseSerilog(Log.Logger));

            return server;
        }
    }
}
