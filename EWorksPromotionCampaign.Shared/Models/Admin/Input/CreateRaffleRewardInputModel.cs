using EWorksPromotionCampaign.Shared.Models.Admin.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Shared.Models.Admin.Input
{
    public class CreateRaffleRewardInputModel
    {
        [Required] public long CampaignId { get; set; }
        [Required] public DateTime StartDate { get; set; }
        [Required] public DateTime EndDate { get; set; }
        [Required] public int NumberofWinners { get; set; }
        public decimal Amount { get; set; }

        public CampaignRaffleReward ToCampaignRaffleReward()
        {
            var campaignRaffleReward = new CampaignRaffleReward()
            {
                CampaignId = CampaignId,
                StartDate = StartDate,
                EndDate = EndDate,
                NumberofWinners = NumberofWinners,
                Amount = Amount
            };
            return campaignRaffleReward;
        }
    }
}
