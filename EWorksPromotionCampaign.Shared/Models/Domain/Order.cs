using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Shared.Models.Domain
{
    public class Order
    {
        public Order(long id)
        {
            Id = id;
        } 
        public Order() {}
        public long Id { get; set; }
        public long UserId { get; set; }
        public decimal Amount { get; set; }
        public long CampaignId { get; set; }
        public string Status { get; set; }
        public string Reference { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime MyProperty { get; set; }
    }
}
