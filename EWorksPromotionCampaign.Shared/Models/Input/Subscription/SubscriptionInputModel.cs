using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Shared.Models.Input.Subscription
{
    public class SubscriptionInputModel
    {
        public long UserId { get; set; }
        [Required] public string PlainCode { get; set; }

    }
}
