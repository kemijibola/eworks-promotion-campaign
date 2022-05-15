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
    public class CreateCampaignInputModel
    {
        [Required] public string Name { get; set; }
        [Required] public DateTime StartDate { get; set; }
        [Required] public DateTime EndDate { get; set; }
        [Required] public decimal MinEntryAmount { get; set; }
        [Required] public decimal MaxEntryAmount { get; set; }

        public Campaign ToCampaign()
        {
            var campaign = new Campaign()
            {
                Name = Name,
                StartDate = StartDate,
                EndDate = EndDate,
                MinEntryAmount = MinEntryAmount,
                MaxEntryAmount = MaxEntryAmount,
                Status = CampaignStatus.inactive.ToString()
            };
            return campaign;
        }
    }
}
