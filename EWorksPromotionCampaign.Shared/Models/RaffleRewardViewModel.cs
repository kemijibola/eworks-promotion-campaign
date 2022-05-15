using EWorksPromotionCampaign.Shared.Models.Admin.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Shared.Models
{
    public class RaffleRewardViewModel
    {
        public long Id { get; set; }
        public long CampaignId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public int NumberofWinners { get; set; }
        public decimal Amount { get; set; }
        public string Result { get; set; }
        public IReadOnlyCollection<RaffleWinnerViewModel> RaffleWinners { get; set; }
        public IReadOnlyCollection<RaffleSubscriberViewModel> Subscribers { get; set; }
    }

    public class RaffleWinnerViewModel
    {
        public long Id { get; set; }
        public string SubscriberPhone { get; set; }
        public decimal AmountWon { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class RaffleSubscriberViewModel
    {
        public long Id { get; set; }
        public string HashedCode { get; set; }
        public string PlainCode { get; set; }
        public string WinningType { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Telco { get; set; }
        public DateTime DateSubscribed { get; set; }
        public string Status { get; set; }
        public long CampaignId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    public class WinnerData
    {
        public string RewardCode { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class FetchRaffleReward
    {
        public long Id { get; set; }
        public long CampaignId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime DateDrawn { get; set; }
        public string Status { get; set; }
        public int NumberofWinners { get; set; }
        public decimal Amount { get; set; }
        public string Result { get; set; }
    }
    public class RaffleWinnerDetailViewModel
    {
        public string Result { get; set; }
        public IReadOnlyCollection<RaffleSubscriberViewModel> Subscribers { get; set; }
    }
}
