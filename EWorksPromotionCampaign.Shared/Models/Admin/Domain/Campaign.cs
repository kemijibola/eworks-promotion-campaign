using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Shared.Models.Admin.Domain
{
    public class Campaign
    {
        public Campaign(long id)
        {
            Id = id;
        }
        public Campaign() {}
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public decimal MinEntryAmount { get; set; }
        public decimal MaxEntryAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public IReadOnlyCollection<CampaignReward> CampaignRewards { get; set; }

        public Campaign WithCampignRewards(IReadOnlyCollection<CampaignReward> campaignRewards)
        {
            CampaignRewards = campaignRewards;
            return this;
        }
    }
}
