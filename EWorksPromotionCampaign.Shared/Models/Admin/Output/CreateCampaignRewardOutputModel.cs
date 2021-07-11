using EWorksPromotionCampaign.Shared.Models.Admin.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Shared.Models.Admin.Output
{
    public class CreateCampaignRewardOutputModel
    {
        private CreateCampaignRewardOutputModel(CampaignReward campaignReward)
        {
            Id = campaignReward.Id;
            CampaignId = campaignReward.CampaignId;
            WinningType = campaignReward.WinningType;
            CampaignType = campaignReward.CampaignType;
            StartMode = campaignReward.StartMode;
            ScheduleType = campaignReward.ScheduleType;
            Amount = campaignReward.Amount;
            StartDate = campaignReward.StartDate;
            EndDate = campaignReward.EndDate;
            Status = campaignReward.Status;
            NumberOfWinners = campaignReward.NumberOfWinners;
            ScheduleWinningRule = campaignReward.ScheduleWinningRule;
            NthSubscriberValue = campaignReward.NthSubscriberValue;
        }
        public long Id { get; set; }
        public long CampaignId { get; set; }
        public string WinningType { get; set; }
        public string CampaignType { get; set; }
        public string StartMode { get; set; }
        public string ScheduleType { get; set; }
        public decimal Amount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public long NumberOfWinners { get; set; }
        public string ScheduleWinningRule { get; set; }
        public int NthSubscriberValue { get; set; }

        public static CreateCampaignRewardOutputModel FromCampaignReward(CampaignReward campaignReward)
        {
            _ = campaignReward ?? throw new ArgumentNullException(nameof(campaignReward));
            return new CreateCampaignRewardOutputModel(campaignReward);
        }
    }
}
