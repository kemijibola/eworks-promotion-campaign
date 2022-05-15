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

        public static string MaskPhone(string input, int start, int end, char maskCharacter)
        {
            var length = end - start;
            var firstPart = input.Substring(0, start);
            var partToMask = input.Substring(start, length);
            var thirdPart = input.Substring(end, input.Length - (firstPart.Length + partToMask.Length));
            int maskCount = 0;
            var mask = "";
            while (maskCount < partToMask.Length)
            {
                mask += maskCharacter;
                maskCount++;
            }
            return firstPart + mask + thirdPart;
        }

        public static string GenerateOrderReference()
        {
            var today = DateTime.UtcNow;
            return $"W-{today:yyyyMMddHHmmss}";
        }
    }
}
