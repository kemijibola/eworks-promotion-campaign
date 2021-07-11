using EWorksPromotionCampaign.Shared.Models.Admin.Domain;
using EWorksPromotionCampaign.Shared.Models.Admin.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Shared.Models
{
    public class HelperModel
    {
        public static Campaign ToCampaign(FetchCampaignOutputModel model)
        {
            var campaign = new Campaign()
            {
                Id = model.Id,
                Name = model.Name,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Status = model.Status,
                MinEntryAmount = model.MinEntryAmount,
                MaxEntryAmount = model.MaxEntryAmount,
                CreatedAt = model.CreatedAt,
                CampaignRewards = model.CampaignRewards
            };
            return campaign;
        }
    }
}
