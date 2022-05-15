using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EWorksPromotionCampaign.Shared.Util.Enums;

namespace EWorksPromotionCampaign.Shared.Util
{
    public class GenerateTokenRequest
    {
        public IDictionary<IdentityClaimType, string> ClaimTypes { get; set; }
    }
}
