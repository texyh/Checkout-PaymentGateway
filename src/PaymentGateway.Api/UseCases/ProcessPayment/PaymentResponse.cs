using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.Api.UseCases.ProcessPayment
{
    public class PaymentResponse
    {
     
        [JsonProperty("paymentId")]
        public string PaymentId { get; set; }

    }
}
