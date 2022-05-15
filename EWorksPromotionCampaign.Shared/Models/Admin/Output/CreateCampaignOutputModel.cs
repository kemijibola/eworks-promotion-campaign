using EWorksPromotionCampaign.Shared.Models.Admin.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Shared.Models.Admin.Output
{
    public class CreateCampaignOutputModel
    {
        private CreateCampaignOutputModel(Campaign campaign)
        {
            Id = campaign.Id;
            Name = campaign.Name;
            StartDate = campaign.StartDate;
            EndDate = campaign.EndDate;
            Status = campaign.Status;
            MinEntryAmount = campaign.MinEntryAmount;
            MaxEntryAmount = campaign.MaxEntryAmount;
        }
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public decimal MinEntryAmount { get; set; }
        public decimal MaxEntryAmount { get; set; }
        public static CreateCampaignOutputModel FromCampaign(Campaign campaign)
        {
            _ = campaign ?? throw new ArgumentNullException(nameof(campaign));
            return new CreateCampaignOutputModel(campaign);
        }
    }
}
