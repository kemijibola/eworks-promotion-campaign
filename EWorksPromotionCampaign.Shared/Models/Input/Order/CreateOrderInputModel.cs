using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EWorksPromotionCampaign.Shared.Models.Domain;

namespace EWorksPromotionCampaign.Shared.Models.Input.Order
{
    public class CreateOrderInputModel
    {
        public long UserId { get; set; }
        [Required] public decimal Amount { get; set; }
        [Required] public long CampaignId { get; set; }

        public Domain.Order ToOrder()
        {
            var order = new Domain.Order()
            {
                UserId = UserId,
                Amount = Amount,
                CampaignId = CampaignId
            };
            return order;
        }
    }
}
