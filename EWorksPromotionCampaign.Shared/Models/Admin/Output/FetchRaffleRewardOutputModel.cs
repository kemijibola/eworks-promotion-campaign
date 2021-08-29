using EWorksPromotionCampaign.Shared.Models.Admin.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Shared.Models.Admin.Output
{
    public class FetchRaffleRewardOutputModel
    {
        public FetchRaffleRewardOutputModel(FetchRaffleReward raffleReward)
        {
            Id = raffleReward.Id;
            CampaignId = raffleReward.CampaignId;
            StartDate = raffleReward.StartDate;
            EndDate = raffleReward.EndDate;
            DateDrawn = raffleReward.DateDrawn;
            Status = raffleReward.Status;
            NumberofWinners = raffleReward.NumberofWinners;
            Amount = raffleReward.Amount;
        }
        public long Id { get; set; }
        public long CampaignId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime DateDrawn { get; set; }
        public string Status { get; set; }
        public int NumberofWinners { get; set; }
        public decimal Amount { get; set; }

        public static FetchRaffleRewardOutputModel FromRaffleReward(FetchRaffleReward raffleReward)
        {
            _ = raffleReward ?? throw new ArgumentNullException(nameof(raffleReward));
            return new FetchRaffleRewardOutputModel(raffleReward);
        }
    }
}
