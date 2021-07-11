using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Shared.Models.Admin.Domain
{
    public class CampaignReward
    {
        public CampaignReward(long id)
        {
            Id = id;
        }
        public CampaignReward() {}
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
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
