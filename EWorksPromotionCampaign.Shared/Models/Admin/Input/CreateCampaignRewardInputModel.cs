using EWorksPromotionCampaign.Shared.Models.Admin.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EWorksPromotionCampaign.Shared.Util.Enums;

namespace EWorksPromotionCampaign.Shared.Models.Admin.Input
{
    public class CreateCampaignRewardInputModel
    {
        [Required] [Range(1, double.PositiveInfinity)] public long CampaignId { get; set; }
        [Required] public string WinningType { get; set; }
        [Required] public string CampaignType { get; set; }
        [Required] public string StartMode { get; set; }
        [Required] public string ScheduleType { get; set; }
        [Required] public decimal Amount { get; set; }
        [Required] public DateTime StartDate { get; set; }
        [Required] public DateTime EndDate { get; set; }
        public string Status { get; set; }
        [Required] public long NumberOfWinners { get; set; }
        [Required] public string ScheduleWinningRule { get; set; }
        public int NthSubscriberValue { get; set; }

        public CampaignReward ToCampaignReward(string status)
        {
            var campaignReward = new CampaignReward
            {
                CampaignId = CampaignId,
                WinningType = WinningType,
                CampaignType = CampaignType,
                StartMode = StartMode,
                ScheduleType = ScheduleType,
                Amount = Amount,
                StartDate = StartDate,
                EndDate = EndDate,
                Status = status,
                NumberOfWinners = NumberOfWinners,
                ScheduleWinningRule = ScheduleWinningRule,
                NthSubscriberValue = NthSubscriberValue
            };
            return campaignReward;
        }
    }
}
