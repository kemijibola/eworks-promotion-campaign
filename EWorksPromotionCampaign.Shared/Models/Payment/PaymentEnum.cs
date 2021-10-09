using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Shared.Models.Payment
{
    public enum PaymentGatewayStatus
    {
        success = 0,
        abandoned,
        failed,
        pending
    }
}
