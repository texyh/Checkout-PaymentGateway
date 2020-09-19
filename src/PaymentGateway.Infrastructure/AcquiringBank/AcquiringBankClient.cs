using Microsoft.Extensions.Options;
using PaymentGateway.Domain.AcquiringBank;
using PaymentGateway.Domain.Helpers;
using PaymentGateway.Domain.Payments;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Infrastructure.AcquiringBank
{
    public class AcquiringBankClient : IAquiringBankClient
    {
        private readonly HttpClient _httpClient;

        private readonly AcquiringBankSettings _acquiringBankSettings;

        public AcquiringBankClient(
            HttpClient httpClient,
            IOptionsMonitor<AcquiringBankSettings> bankSettings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(bankSettings.CurrentValue.ApiUrl);
            _acquiringBankSettings = bankSettings.CurrentValue;
        }

        public async Task<BankPaymentResponse> ProcessPayment(BankPaymentRequest requestModel)
        {
            using var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"{_acquiringBankSettings.ApiUrl}/api/payment"),
                Method = HttpMethod.Post,
                Content = requestModel.ToStringContent()
            };

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return new BankPaymentResponse
                {
                    PaymentStatus = PaymentStatus.Failed
                };
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            return responseContent.FromJson<BankPaymentResponse>();
        }
    }
}
