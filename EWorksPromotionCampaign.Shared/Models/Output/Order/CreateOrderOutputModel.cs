using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Shared.Models.Output.Order
{
    public class CreateOrderOutputModel
    {
        private CreateOrderOutputModel(Domain.Order order)
        {
            Id = order.Id;
            UserId = order.UserId;
            Amount = order.Amount;
            CampaignId = order.CampaignId;
            Status = order.Status;
            Reference = order.Reference;
        }
        public long Id { get; set; }
        public long UserId { get; set; }
        public decimal Amount { get; set; }
        public long CampaignId { get; set; }
        public string Status { get; set; }
        public string Reference { get; set; }
        public static CreateOrderOutputModel FromOrder(Domain.Order order)
        {
            _ = order ?? throw new ArgumentNullException(nameof(order));
            return new CreateOrderOutputModel(order);
        }
    }
}
