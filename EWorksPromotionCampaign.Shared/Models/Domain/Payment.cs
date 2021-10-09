using System;

namespace EWorksPromotionCampaign.Shared.Models.Domain
{
    public class Payment
    {
        public Payment(long id)
        {
            Id = id;
        }
        public Payment() {}
        public long Id { get; set; }
        public long OrderId { get; set; }
        public long Amount { get; set; }
        public string Description { get; set; }
        public string GatewayReponse { get; set; }
        public string GatewayReference { get; set; }
        public bool Status { get; set; }
        public string PaymentStatus { get; set; }
        public string AdditionalInfo { get; set; }
        public DateTime TransactionDate { get; set; }

    }
}
