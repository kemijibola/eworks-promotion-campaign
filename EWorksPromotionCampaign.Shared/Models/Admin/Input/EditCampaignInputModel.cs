using EWorksPromotionCampaign.Shared.Models.Admin.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Shared.Models.Admin.Input
{
    public class EditCampaignInputModel
    {
        public long Id { get; set; }
        [Required] public string Name { get; set; }
        [Required] public DateTime StartDate { get; set; }
        [Required] public DateTime EndDate { get; set; }
        [Required] public decimal MinEntryAmount { get; set; }
        [Required] public decimal MaxEntryAmount { get; set; }

        public Campaign ToCampaign()
        {
            var campaign = new Campaign(Id)
            {
                Name = Name,
                StartDate = StartDate,
                EndDate = EndDate,
                MinEntryAmount = MinEntryAmount,
                MaxEntryAmount = MaxEntryAmount
            };
            return campaign;
        }
    }
}
