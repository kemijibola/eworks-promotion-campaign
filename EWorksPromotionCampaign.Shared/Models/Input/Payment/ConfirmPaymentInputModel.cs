using EWorksPromotionCampaign.Shared.Models.Payment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Shared.Models.Input.Payment
{
    public class ConfirmPaymentInputModel
    {
        [Required] [Range(1, 99999999)] public long OrderId { get; set; }
        [Required] public string Reference { get; set; }
        public string CustomerEmail { get; set; }
        public Domain.Payment ToPayment(PaystackPaymentVerificationResponse paystackResponse)
        {
            Domain.Payment payment = new()
            {
                OrderId = OrderId,
                Amount = Convert.ToInt64(paystackResponse?.Data.Amount) / 100,
                Description = paystackResponse.Message,
                Status = paystackResponse.Status,
                PaymentStatus = paystackResponse.Data.Status,
                GatewayReference = paystackResponse.Data.Reference,
                GatewayReponse = paystackResponse.Data.GatewayResponse,
                TransactionDate = Convert.ToDateTime(paystackResponse.Data.TransactionDate),
                AdditionalInfo = $"IpAddress-{paystackResponse.Data.IpAddress}: Channel-{paystackResponse.Data.Channel}: Customer- {paystackResponse?.Data.Customer.Email}"
            };
            return payment;
        }
    }
}
