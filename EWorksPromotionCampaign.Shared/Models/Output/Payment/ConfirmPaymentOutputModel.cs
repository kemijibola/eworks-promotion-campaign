using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Shared.Models.Output.Payment
{
    public class ConfirmPaymentOutputModel
    {
        private ConfirmPaymentOutputModel(Domain.Payment payment)
        {
            Id = payment.Id;
            OrderId = payment.OrderId;
            Amount = payment.Amount;
            Description = payment.Description;
            Reponse = payment.GatewayReponse;
            Reference = payment.GatewayReference;
            Status = payment.Status;
            TransactionDate = payment.TransactionDate;
        }
        public long Id { get; set; }
        public long OrderId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string Reponse { get; set; }
        public string Reference { get; set; }
        public bool Status { get; set; }
        public DateTime TransactionDate { get; set; }
        public static ConfirmPaymentOutputModel FromPayment(Domain.Payment payment)
        {
            _ = payment ?? throw new ArgumentNullException(nameof(payment));
            return new ConfirmPaymentOutputModel(payment);
        }
    }
}
