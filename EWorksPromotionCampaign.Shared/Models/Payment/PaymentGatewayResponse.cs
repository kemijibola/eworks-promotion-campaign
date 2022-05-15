using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Shared.Models.Payment
{
    public class PaystackPaymentVerificationResponse
    {
        [JsonProperty(PropertyName = "status")] public bool Status { get; set; }
        [JsonProperty(PropertyName = "message")] public string Message { get; set; }
        [JsonProperty(PropertyName = "data")] public PaystackPaymentData Data { get; set; }

    }

    public class PaystackPaymentData
    {
        [JsonProperty(PropertyName = "amount")] public long Amount { get; set; }
        [JsonProperty(PropertyName = "gateway_response")] public string GatewayResponse { get; set; }
        [JsonProperty(PropertyName = "transaction_date")] public string TransactionDate { get; set; }
        [JsonProperty(PropertyName = "status")] public string Status { get; set; }
        [JsonProperty(PropertyName = "reference")] public string Reference { get; set; }
        [JsonProperty(PropertyName = "ip_address")] public string IpAddress { get; set; }
        [JsonProperty(PropertyName = "channel")] public string Channel { get; set; }
        [JsonProperty(PropertyName = "customer")] public PaystackPaymentCustomer Customer { get; set; }
    }

    public class PaystackPaymentCustomer
    {
        [JsonProperty(PropertyName = "id")] public long Id { get; set; }
        [JsonProperty(PropertyName = "email")] public string Email { get; set; }
    }
}
