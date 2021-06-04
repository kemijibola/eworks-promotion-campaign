using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Service.Services.External
{
    public class ExternalServicesConfig
    {
        public const string QuickTellerServiceApi = "QuickTellerServiceApi";
        public const string PaystackServiceApi = "PaystackServiceApi";
        public string Url { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}
