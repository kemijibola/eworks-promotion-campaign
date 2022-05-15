using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Shared.Models.Admin.Domain
{
    public class CampaignRaffleReward
    {
        public CampaignRaffleReward(long id)
        {
            Id = id;
        }
        public CampaignRaffleReward() {}
        public long Id { get; set; }
        public long CampaignId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime DateDrawn { get; set; }
        public string Status { get; set; }
        public int NumberofWinners { get; set; }
        public decimal Amount { get; set; }
    }
}
